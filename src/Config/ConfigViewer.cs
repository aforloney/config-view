using System.Runtime.CompilerServices;
using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config
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