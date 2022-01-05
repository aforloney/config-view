using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ConfigView.Tests.Fakes
{
    public class FakeConfiguration : IConfiguration
    {
        private IEnumerable<IConfigurationSection> Sections { get; set; } = Enumerable.Empty<IConfigurationSection>();
        public IEnumerable<IConfigurationSection> GetChildren() => Sections;

        # region Not needed implementations
        public string this[string key] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public IChangeToken GetReloadToken() => throw new System.NotImplementedException();

        public IConfigurationSection GetSection(string key) => throw new System.NotImplementedException();

        # endregion
        public IConfigurationRoot ConfigureRoot(string json)
        {
            var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            var configRoot = new ConfigurationBuilder().AddJsonStream(stream).Build();
            ConfigureSections(configRoot);
            return configRoot;
        }

        public IConfigurationRoot ConfigureRoot(IDictionary<string, string> initialData)
        {
            var configRoot = new ConfigurationBuilder().AddInMemoryCollection(initialData).Build();
            ConfigureSections(configRoot);
            return configRoot;
        }

        public IConfigurationRoot ConfigureRoot(string json, IDictionary<string, string> initialData)
        {
            var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            var configRoot = new ConfigurationBuilder()
                                    .AddJsonStream(stream)
                                    .AddInMemoryCollection(initialData).Build();
            ConfigureSections(configRoot);
            return configRoot;
        }

        public IConfigurationRoot ConfigureRootEnvironmentVariables(IDictionary<string,string> environmentVariables,
                string prefix = "")
        {
            foreach (var env in environmentVariables)
            {
                Environment.SetEnvironmentVariable($"{prefix}{env.Key}", env.Value);
            }
            var configRoot = new ConfigurationBuilder()
                                    .AddEnvironmentVariables(prefix)
                                    .Build();
            ConfigureSections(configRoot);
            return configRoot;
        }

        private void ConfigureSections(IConfigurationRoot root) =>
            Sections = new List<ConfigurationSection>
            {
                new ConfigurationSection(root, string.Empty)
            };
    }
}