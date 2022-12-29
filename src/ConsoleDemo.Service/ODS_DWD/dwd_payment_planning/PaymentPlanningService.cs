using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using ConsoleDemo.Domain;
using ConsoleDemo.Infrastructure;

namespace ConsoleDemo.Service.ODS_DWD.dwd_payment_planning
{
    public class PaymentPlanningService : IEtlHostedService
    {
        public string Name => "dwd_payment_planning";
        private readonly AppOptions _options;

        public PaymentPlanningService(AppOptions options)
        {
            _options = options;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var dateInterval = DateTimeOffset.Now.AddMinutes(-10);
            var timeStamp = EnvironmentVariablesExtension.TimeStamp;
            if (timeStamp > 0)
            {
                dateInterval = timeStamp.ToString().Length == 13
                    ? DateTimeOffset.FromUnixTimeMilliseconds(timeStamp)
                    : DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            }

            await using var conn = new MySqlConnection(_options.ConnectionString);

            var hasSalesDepartment = await CheckSalesDepartmentAsync(conn, DateTimeOffset.Now);
            if (!hasSalesDepartment)
            {
                throw new ApplicationException("还没有销售组织架构月度备份数据");
            }

            //插入数据
            await InsertAsync(conn);
            //更新数据
            await UpdateAsync(conn, dateInterval);
        }

        private async Task InsertAsync(IDbConnection conn)
        {
            var sql = @$"insert into bi.dwd_payment_planning(planningId,staffId,deptId,clientId,dt,amount,modifyType)
             select p.Id,p.StaffId, s.DeptId,p.ClientId,p.ExpectedDate,p.ExpectedAmount,if(Deleted=0,0,1) ModifyType
             from crm.PaymentPlanning p
             left join crm.SalesDepartmentStaffMonthlySnapshot s on p.StaffId = s.StaffId and s.SnapshotMonth = DATE_FORMAT(p.ExpectedDate,'%Y%m')
             where not exists (select 1 from bi.dwd_payment_planning where planningId = p.Id);";

            await conn.ExecuteAsync(sql);
        }

        public async Task UpdateAsync(IDbConnection conn, DateTimeOffset date)
        {
            var sql = @$"with TPlanning as (
             select p.Id,p.StaffId, s.DeptId,p.ClientId,p.ExpectedDate,p.ExpectedAmount,if(Deleted=0,0,1) ModifyType 
             from crm.PaymentPlanning p
             left join crm.SalesDepartmentStaffMonthlySnapshot s on p.StaffId = s.StaffId and s.SnapshotMonth = DATE_FORMAT(p.ExpectedDate,'%Y%m')
             where p.LastModificationTime > @modifyDate
             )
             update bi.dwd_payment_planning dwd,TPlanning t 
             set dwd.staffId = t.StaffId,
             dwd.deptId =t.DeptId,
             dwd.clientId=t.ClientId,
             dwd.dt = t.ExpectedDate,
             dwd.amount = t.ExpectedAmount,
             dwd.modifyType = t.ModifyType
             where dwd.planningId = t.Id;";

            await conn.ExecuteAsync(sql, new {modifyDate = date.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss")});
        }

        /// <summary>
        /// 检测月度组织架构备份数据
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private async Task<bool> CheckSalesDepartmentAsync(IDbConnection conn, DateTimeOffset date)
        {
            var sql = @$"SELECT 1 FROM crm.SalesDepartmentStaffMonthlySnapshot where SnapshotMonth=@SnapshotMonth;";
            var data = await conn.QueryFirstOrDefaultAsync(sql, new {SnapshotMonth = date.ToLocalTime().ToString("yyyyMM")});
            return data != null;
        }
    }
}