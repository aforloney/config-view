using Microsoft.Extensions.DependencyInjection;
using ConfigView.Config.Viewers;
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

        public static void AddEnvironmentConfigEndpoint(this IServiceCollection services)
        {
            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IConfigViewer, EnvironmentVariableConfigViewer>();
        }

        public static void AddMemoryConfigEndpoint(this IServiceCollection services)
        {
            services.AddTransient<IContentProvider, ContentProvider>();
            services.AddTransient<IConfigViewer, MemoryConfigViewer>();
        }
    }
}