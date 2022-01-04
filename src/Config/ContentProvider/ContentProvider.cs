using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config.ContentProvider
{
    internal class ContentProvider : IContentProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ContentProvider(IConfiguration configuration)
        {
            _configurationRoot = configuration as IConfigurationRoot ?? new ConfigurationBuilder().Build();
        }

        public IEnumerable<ConfigView> Get(IEnumerable<Type> providerTypes)
        {
            void RecurseChildren(IList<ConfigView> configs,
                ConfigView config,
                IEnumerable<IConfigurationSection> children)
            {
                foreach (IConfigurationSection child in children)
                {
                    var valueAndProvider = GetValueAndProvider(_configurationRoot, child.Path, providerTypes);
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

            RecurseChildren(conigfData, currentConfig, _configurationRoot.GetChildren());

            return conigfData;
        }

        private static Provider GetValueAndProvider(
            IConfigurationRoot root,
            string key,
            IEnumerable<Type> providerTypes)
        {
            var providerValue = new Provider();
            foreach (IConfigurationProvider provider in root.Providers.Reverse())
            {
                if (providerTypes.Any(t => t.IsAssignableFrom(provider.GetType())))
                {
                    if (provider.TryGet(key, out string value))
                    {
                        return new Provider { Source = provider.ToString(), Value = value };
                    }
                }
            }

            return providerValue;
        }
    }
}