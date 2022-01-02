using Microsoft.Extensions.Configuration;

namespace Confv.Config
{
    internal record ConfigView(string Key, string Path, ProviderValue Value)
    {
        public ConfigView() : this("Root", string.Empty, new ProviderValue()) {}
    }

    internal class ProviderValue
    {
        public string? Value { get; set; }
        public IConfigurationProvider? Provider { get; set; }
    }
}