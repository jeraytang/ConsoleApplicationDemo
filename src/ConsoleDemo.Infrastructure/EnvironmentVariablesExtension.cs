using System;

namespace ConsoleDemo.Infrastructure
{
    public static class EnvironmentVariablesExtension
    {
        /// <summary>
        /// 环境变量：服务名称
        /// </summary>
        public static string ServiceName => GetEnvironmentVariable(EnvironmentVariables.ServiceName);

        /// <summary>
        /// 环境变量：时间戳
        /// </summary>
        public static long TimeStamp
        {
            get
            {
                var timeStamp = GetEnvironmentVariable(EnvironmentVariables.TimeStamp);
                if (timeStamp == null)
                {
                    return 0;
                }

                return long.TryParse(timeStamp, out var stamp) ? stamp : 0;
            }
        }

        /// <summary>
        /// 环境变量：全量标识
        /// </summary>
        public static bool IsAll
        {
            get
            {
                var isAll = GetEnvironmentVariable(EnvironmentVariables.IsAll);
                if (isAll == null)
                {
                    return false;
                }

                return isAll.ToLower() == "true" || isAll == "1";
            }
        }

        public static string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(EnvironmentVariables.ServiceName);
        }
    }
}