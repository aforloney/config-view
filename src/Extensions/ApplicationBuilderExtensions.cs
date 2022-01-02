using System.Text.Json;
using Confv.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Confv.Extensions
{
    public static class ApplicationBuilderExtensions
    {
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
    }
}