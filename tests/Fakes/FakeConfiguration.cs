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
            Sections = new List<ConfigurationSection>
            {
                new ConfigurationSection(configRoot, string.Empty)
            };
            return configRoot;
        }
    }
}