namespace ConsoleDemo.Service
{
    public interface IEltHostedServiceFactory
    {
        /// <summary>
        /// 通过名称获取服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        IEtlHostedService GetService(string serviceName);
    }
}