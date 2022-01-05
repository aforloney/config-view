using System.Text.Json;
using ConfigView.Config.Viewers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigView.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// For ASP.NET Core 5 uses
        /// </summary>
        /// <param name="app"></param>
        public static void AddConfigEndpoint(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/config", ctx =>
                {
                    var configViewer = app.ApplicationServices.GetService(typeof(IConfigViewer)) as IConfigViewer;
                    if (configViewer == null)
                        return ctx.Response.WriteAsync(string.Empty);
                    return ctx.Response.WriteAsync(JsonSerializer.Serialize(configViewer.Get()));
                });
            });
        }

        /// <summary>
        /// For ASP.NET Core 6 usage
        /// </summary>
        /// <param name="app"></param>
        /// <param name="builder"></param>
        public static void AddConfigEndpoint(this WebApplication app, WebApplicationBuilder builder)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/config", ctx =>
                {
                    using (var serviceProvider = builder.Services.BuildServiceProvider())
                    {
                        var configViewer = serviceProvider.GetRequiredService<IConfigViewer>();
                        return ctx.Response.WriteAsync(JsonSerializer.Serialize(configViewer.Get()));
                    }
                });
            });
        }
    }
}