using System;
using System.Collections.Generic;
using System.Linq;
using ConfigView.Config.ContentProvider;

namespace ConfigView.Tests.Fakes
{
    internal class FakeContentProvider : IContentProvider
    {
        public IEnumerable<Config.ConfigView> Configs { get; set; } = Enumerable.Empty<Config.ConfigView>();
        public IEnumerable<Config.ConfigView> Get(IEnumerable<Type> providerTypes) => Configs;
        public void SetConfigs(IEnumerable<Config.ConfigView> contents) => Configs = contents;
    }
}