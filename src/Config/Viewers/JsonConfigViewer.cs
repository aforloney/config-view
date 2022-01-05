using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration.Json;

namespace ConfigView.Config.Viewers
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