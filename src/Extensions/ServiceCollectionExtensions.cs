using Microsoft.Extensions.DependencyInjection;
using ConfigView.Config;
using ConfigView.Config.ContentProvider;

namespace ConfigView.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static void AddConfigEndpoint(this IServiceCollection services)
        {
            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IConfigViewer, ConfigViewer>();
        }

        public static void AddJsonConfigEndpoint(this IServiceCollection services)
        {
            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IConfigViewer, JsonConfigViewer>();
        }
    }
}