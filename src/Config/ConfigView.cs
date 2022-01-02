using Microsoft.Extensions.Configuration;

namespace Confv.Config
{
    internal record ConfigView(string Key, string Path, Provider ProviderDetails)
    {
        public ConfigView() : this(string.Empty, string.Empty, new Provider()) {}
    }

    internal class Provider
    {
        public string? Source { get; set; }
        public string? Value { get; set; }
    }
}