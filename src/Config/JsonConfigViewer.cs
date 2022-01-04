using System.Runtime.CompilerServices;
using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration.Json;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config
{
    internal class JsonConfigViewer : IConfigViewer
    {
        private readonly IContentProvider _contentProvider;

        public JsonConfigViewer(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IEnumerable<ConfigView> Get() => _contentProvider.Get(new[] {
            typeof(JsonConfigurationProvider),
            typeof(JsonStreamConfigurationProvider)
        });
    }
}