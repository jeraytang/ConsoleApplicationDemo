using Microsoft.Extensions.Configuration;

namespace ConsoleDemo.Domain
{
    public class AppOptions
    {
        private readonly IConfiguration _configuration;

        public AppOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration["ConnectionStrings:ConnectionString"];
    }
}