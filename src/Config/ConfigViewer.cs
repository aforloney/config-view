using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config
{
    internal class ConfigViewer : IConfigViewer
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConfigViewer(IConfiguration configuration)
        {
            _configurationRoot = configuration as IConfigurationRoot ?? new ConfigurationBuilder().Build();
        }

        public IEnumerable<ConfigView> Get()
        {
            return GetContents(_configurationRoot);
        }

        private IEnumerable<ConfigView> GetContents(IConfigurationRoot root)
        {
            void RecurseChildren(IList<ConfigView> configs,
                ConfigView config,
                IEnumerable<IConfigurationSection> children)
            {
                foreach (IConfigurationSection child in children)
                {
                    var valueAndProvider = GetValueAndProvider(root, child.Path);
                    config = config with { Key = child.Key, Path = child.Path };
                    if (valueAndProvider.Source != null)
                    {
                        config = config with { ProviderDetails = valueAndProvider };
                        configs.Add(config);
                    }

                    RecurseChildren(configs, config, child.GetChildren());
                }
            }

            List<ConfigView> conigfData = new();
            ConfigView currentConfig = new();

            RecurseChildren(conigfData, currentConfig, root.GetChildren());

            return conigfData;
        }

        private static Provider GetValueAndProvider(
            IConfigurationRoot root,
            string key)
        {
            var providerValue = new Provider();
            foreach (IConfigurationProvider provider in root.Providers.Reverse())
            {
                if (provider.TryGet(key, out string value))
                {
                    return new Provider { Source = provider.ToString(), Value = value };
                }
            }

            return providerValue;
        }
    }
}