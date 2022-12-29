using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDemo.Service
{
    public interface IEtlHostedService
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name => GetType().Name;

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        Task StartAsync(CancellationToken cancellationToken);
    }
}