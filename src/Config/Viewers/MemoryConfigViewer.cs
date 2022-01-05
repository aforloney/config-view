using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration.Memory;

namespace ConfigView.Config.Viewers
{
    internal class MemoryConfigViewer : IConfigViewer
    {
        private readonly IContentProvider _contentProvider;

        public MemoryConfigViewer(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IEnumerable<ConfigView> Get() => _contentProvider.Get(new[] {
            typeof(MemoryConfigurationProvider)
        });
    }
}