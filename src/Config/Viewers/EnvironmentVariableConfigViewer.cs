using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace ConfigView.Config.Viewers
{
    internal class EnvironmentVariableConfigViewer : IConfigViewer
    {
        private readonly IContentProvider _contentProvider;

        public EnvironmentVariableConfigViewer(IContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public IEnumerable<ConfigView> Get() => _contentProvider.Get(new[] {
            typeof(EnvironmentVariablesConfigurationProvider)
        });
    }
}