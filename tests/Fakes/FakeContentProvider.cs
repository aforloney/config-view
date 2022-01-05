using System;
using System.Collections.Generic;
using System.Linq;
using ConfigView.Config.ContentProvider;

namespace ConfigView.Tests.Fakes
{
    internal class FakeContentProvider : IContentProvider
    {
        public IEnumerable<Config.ConfigView> Configs { get; set; } = Enumerable.Empty<Config.ConfigView>();
        public IEnumerable<Config.ConfigView> Get(IEnumerable<Type> providerTypes)
            => Configs
                .Where(cfg => providerTypes.Any(t => t.IsAssignableFrom(TypeFromProviderSource(cfg.ProviderDetails.Source!))))
                .Select(c => c)
                .ToList();
        public void SetConfigs(IEnumerable<Config.ConfigView> contents) => Configs = contents;

        /// <summary>
        /// Returns the Type which represents the type in string form
        /// e.g, "IConfigurationProvider" => typeof(IConfigurationProvider)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private Type? TypeFromProviderSource(string source)
            => AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name.Equals(source));
    }
}