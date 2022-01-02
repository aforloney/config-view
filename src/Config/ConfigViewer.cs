using Microsoft.Extensions.Configuration;

namespace Confv.Config
{
    internal class ConfigViewer : IConfigViewer
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConfigViewer(IConfiguration configuration)
        {
            _configurationRoot = configuration as ConfigurationRoot ?? new ConfigurationBuilder().Build();
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
                    if (valueAndProvider.Provider != null)
                    {
                        config = config with { Value = valueAndProvider };
                        configs.Add(config);
                    }
                    else
                    {
                        config = config with
                        {
                            Path = config.Key + (!string.IsNullOrEmpty(config.Key) ? ":" : string.Empty) + child.Key
                        };
                    }

                    RecurseChildren(configs, config, child.GetChildren());
                }
            }

            List<ConfigView> conigfData = new();
            ConfigView currentConfig = new();

            RecurseChildren(conigfData, currentConfig, root.GetChildren());

            return conigfData;
        }

        private static ProviderValue GetValueAndProvider(
            IConfigurationRoot root,
            string key)
        {
            var providerValue = new ProviderValue();
            foreach (IConfigurationProvider provider in root.Providers.Reverse())
            {
                if (provider.TryGet(key, out string value))
                {
                    return new ProviderValue { Value = value, Provider = provider };
                }
            }

            return providerValue;
        }
    }
}