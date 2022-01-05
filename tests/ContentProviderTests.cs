using Xunit;
using ConfigView.Config;
using ConfigView.Tests.Fakes;
using System.Collections.Generic;
using Shouldly;
using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;
using System;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Linq;

namespace ConfigView.Tests
{
    public class ContentProviderTests : IDisposable
    {
        private IContentProvider Subject { get; set; }
        private FakeConfiguration FakeConfiguration { get; }

        private IDictionary<string, string> EnvironmentVariables { get; set; }

        public ContentProviderTests()
        {
            FakeConfiguration = new FakeConfiguration();
            EnvironmentVariables = new Dictionary<string, string>();
            Subject = new ContentProvider(FakeConfiguration);
        }

        [Fact]
        public void When_NoConfigurationAllRequested_Then_Empty()
        {
            // Arrange
            // intentionally blank
            // Act
            var config = Subject.Get(new [] { typeof(IConfigurationProvider) });

            // Assert
            config.ShouldBeEmpty();
        }

        [Fact]
        public void When_MemoryConfigSimpleConfigurationsSuppliedAllRequested_Then_Contents_Returned()
        {
            // Arrange
            var initialData = new Dictionary<string, string> { { "KeyA", "ValueA" } };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRoot(initialData));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("KeyA", "KeyA", new Provider
                {
                    Source = "MemoryConfigurationProvider",
                    Value = "ValueA"
                }),
             };

            // Act
            var actual = Subject.Get(new[] { typeof(MemoryConfigurationProvider) });

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_MemoryConfigMultipleConfigurationsSuppliedAllRequested_Then_Contents_Returned()
        {
            // Arrange
            var initialData = new Dictionary<string, string> { { "KeyA", "ValueA" }, { "KeyB", "ValueB" } };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRoot(initialData));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("KeyA", "KeyA", new Provider
                {
                    Source = "MemoryConfigurationProvider",
                    Value = "ValueA"
                }),
                new Config.ConfigView("KeyB", "KeyB", new Provider
                {
                    Source = "MemoryConfigurationProvider",
                    Value = "ValueB"
                }),
             };

            // Act
            var actual = Subject.Get(new[] { typeof(MemoryConfigurationProvider) });

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_MultipleConfigurationsSuppliedAllRequested_Then_Contents_Returned()
        {
            // Arrange
            var initialData = new Dictionary<string, string> { { "KeyA", "ValueA" } };
            var jsonString = @"{""KeyB"":""ValueB""}";
            Subject = new ContentProvider(FakeConfiguration.ConfigureRoot(jsonString, initialData));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("KeyA", "KeyA", new Provider
                {
                    Source = "MemoryConfigurationProvider",
                    Value = "ValueA"
                }),
                new Config.ConfigView("KeyB", "KeyB", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "ValueB"
                }),
             };

            // Act
            var actual = Subject.Get(new [] { typeof(IConfigurationProvider) });

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_MultipleConfigurationsSuppliedJsonRequested_Then_Contents_Returned()
        {
            // Arrange
            var initialData = new Dictionary<string, string> { { "KeyA", "ValueA" } };
            var jsonString = @"{""KeyB"":""ValueB""}";
            Subject = new ContentProvider(FakeConfiguration.ConfigureRoot(jsonString, initialData));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("KeyB", "KeyB", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "ValueB"
                }),
             };

            // Act
            var actual = Subject.Get(new[] { typeof(JsonStreamConfigurationProvider) });

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_EnvironmentVariableNoPrefixSingleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            EnvironmentVariables = new Dictionary<string, string> { { "MyEnvironmentVariableKeyA", "ValueA" } };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRootEnvironmentVariables(EnvironmentVariables));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("MyEnvironmentVariableKeyA", "MyEnvironmentVariableKeyA", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: ''",
                    Value = "ValueA"
                }),
             };

            // Act
            var actual = Subject.Get(new[] { typeof(EnvironmentVariablesConfigurationProvider) });

            // Assert
            foreach (var exp in expected)
            {
                actual.Any(act => act.Key == exp.Key
                               && act.Path == exp.Path
                               && act.ProviderDetails.Source == exp.ProviderDetails.Source
                               && act.ProviderDetails.Value == exp.ProviderDetails.Value).ShouldBeTrue();
            }
        }

        [Fact]
        public void When_EnvironmentVariableNoPrefixMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            EnvironmentVariables = new Dictionary<string, string> {
                    { "MyEnvironmentVariableKeyA", "ValueA" },
                    { "MyEnvironmentVariableKeyB", "ValueB" },
                    };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRootEnvironmentVariables(EnvironmentVariables));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("MyEnvironmentVariableKeyA", "MyEnvironmentVariableKeyA", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: ''",
                    Value = "ValueA"
                }),
                new Config.ConfigView("MyEnvironmentVariableKeyB", "MyEnvironmentVariableKeyB", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: ''",
                    Value = "ValueB"
                })
             };

            // Act
            var actual = Subject.Get(new[] { typeof(EnvironmentVariablesConfigurationProvider) });

            // Assert
            foreach (var exp in expected)
            {
                actual.Any(act => act.Key == exp.Key
                               && act.Path == exp.Path
                               && act.ProviderDetails.Source == exp.ProviderDetails.Source
                               && act.ProviderDetails.Value == exp.ProviderDetails.Value).ShouldBeTrue();
            }
        }

        [Fact]
        public void When_EnvironmentVariableWithPrefixSingleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            EnvironmentVariables = new Dictionary<string, string> { { "MyEnvironmentVariableKeyA", "ValueA" } };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRootEnvironmentVariables(EnvironmentVariables, "TEST:"));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("MyEnvironmentVariableKeyA", "MyEnvironmentVariableKeyA", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: 'TEST:'",
                    Value = "ValueA"
                }),
             };

            // Act
            var actual = Subject.Get(new[] { typeof(EnvironmentVariablesConfigurationProvider) });

            // Assert
            foreach (var exp in expected)
            {
                actual.Any(act => act.Key == exp.Key
                               && act.Path == exp.Path
                               && act.ProviderDetails.Source == exp.ProviderDetails.Source
                               && act.ProviderDetails.Value == exp.ProviderDetails.Value).ShouldBeTrue();
            }
        }

        [Fact]
        public void When_EnvironmentVariableWithPrefixMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            EnvironmentVariables = new Dictionary<string, string> {
                    { "MyEnvironmentVariableKeyA", "ValueA" },
                    { "MyEnvironmentVariableKeyB", "ValueB" },
                    };
            Subject = new ContentProvider(FakeConfiguration.ConfigureRootEnvironmentVariables(EnvironmentVariables, "TEST:"));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("MyEnvironmentVariableKeyA", "MyEnvironmentVariableKeyA", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: 'TEST:'",
                    Value = "ValueA"
                }),
                new Config.ConfigView("MyEnvironmentVariableKeyB", "MyEnvironmentVariableKeyB", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider Prefix: 'TEST:'",
                    Value = "ValueB"
                })
             };

            // Act
            var actual = Subject.Get(new[] { typeof(EnvironmentVariablesConfigurationProvider) });

            // Assert
            foreach (var exp in expected)
            {
                actual.Any(act => act.Key == exp.Key
                               && act.Path == exp.Path
                               && act.ProviderDetails.Source == exp.ProviderDetails.Source
                               && act.ProviderDetails.Value == exp.ProviderDetails.Value).ShouldBeTrue();
            }
        }

        public void Dispose()
        {
            foreach (var key in EnvironmentVariables.Keys)
                Environment.SetEnvironmentVariable(key, null);
        }
    }
}