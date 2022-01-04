using Microsoft.Extensions.DependencyInjection;
using ConfigView.Config;

namespace ConfigView.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static void AddConfigEndpoint(this IServiceCollection services) =>
            services.AddTransient<IConfigViewer, ConfigViewer>();
        public static void AddJsonConfigEndpoint(this IServiceCollection services) =>
            services.AddTransient<IConfigViewer, JsonConfigViewer>();
    }
}