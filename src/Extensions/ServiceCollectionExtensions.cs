using Microsoft.Extensions.DependencyInjection;
using Confv.Config;

namespace Confv.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static void AddConfigEndpoint(this IServiceCollection services)
        {
            services.AddTransient<IConfigViewer, ConfigViewer>();
        }
    }
}