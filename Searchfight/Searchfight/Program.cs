using Data.AgentService.Agents.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Util.IOC;

namespace Searchfight
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<App>().StartQuerySearch().Wait();
        }

        public static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            var config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddTransient<App>();
            IConfigurationRoot configuration = (IConfigurationRoot)config;


            services.Configure<WsOptions>(configuration.GetSection("APIINFO"));

            DependencyInjectorBootstraper.RegisterServices(services, config);

            return services;
        }
    }
}
