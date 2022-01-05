using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration;

namespace ConfigView.Config.Viewers
{
    internal class ConfigViewer : IConfigViewer
    {
        private readonly IContentProvider _contentProvider;

        public ConfigViewer(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IEnumerable<ConfigView> Get() => _contentProvider.Get(new[] { typeof(IConfigurationProvider) });
    }
}