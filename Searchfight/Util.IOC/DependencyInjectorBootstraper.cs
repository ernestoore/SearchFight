using Data.AgentService.Agents;
using Data.AgentService.Resilience.Http;
using Domain.AgentContrats;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Task.Interfaces;
using Task.Services;

namespace Util.IOC
{
    public class DependencyInjectorBootstraper
    {
        public static void  RegisterServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<ISearchEngineService, SearchEngineService>();
            services.AddScoped<IServiceSearchAgent, ServiceSearchAgent>();



            services.AddSingleton<IResilientHttpClientFactory, ResilientHttpClientFactory>(sp =>
            {
                //var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();

                var retryCount = 6; // Move to setting
                var exceptionsAllowedBeforeBreaking = 5; // Move to setting;

                return new ResilientHttpClientFactory(exceptionsAllowedBeforeBreaking, retryCount);
            });
            services.AddSingleton<IHttpClient, ResilientHttpClient>(sp => sp.GetService<IResilientHttpClientFactory>().CreateResilientHttpClient());
        }
    }
}
