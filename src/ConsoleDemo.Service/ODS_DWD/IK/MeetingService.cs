using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDemo.Service.ODS_DWD.IK
{
    /// <summary>
    /// 产业真知数据清洗
    /// </summary>
    public class MeetingService : IEtlHostedService
    {
        public string Name => "ik-meeting";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("产业真知清洗数据了~~~");
            return Task.CompletedTask;
        }
    }
}