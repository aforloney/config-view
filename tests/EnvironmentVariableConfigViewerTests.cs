using Xunit;
using ConfigView.Config;
using ConfigView.Config.Viewers;
using ConfigView.Tests.Fakes;
using System.Linq;
using System.Collections.Generic;
using Shouldly;

namespace ConfigView.Tests
{
    public class EnvironmentVariableConfigViewerTests
    {
        private IConfigViewer Subject { get; set; }
        private FakeContentProvider FakeContentProvider { get; }

        public EnvironmentVariableConfigViewerTests()
        {
            FakeContentProvider = new FakeContentProvider();
            Subject = new EnvironmentVariableConfigViewer(FakeContentProvider);
        }

        [Fact]
        public void When_NoConfiguration_Then_Empty()
        {
            // Arrange
            // intentionally blank
            // Act
            var config = Subject.Get();

            // Assert
            Assert.Equal(config, Enumerable.Empty<Config.ConfigView>());
        }

        [Fact]
        public void When_NonEnvironmentVariableConfigurationSupplied_Then_Empty()
        {
            // Arrange
            var configView = new Config.ConfigView("Key", "Key", new Provider
                {
                    Source = "OtherProvider",
                    Value = "Value1"
                });
            var expected = new List<Config.ConfigView> { configView };
            FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            actual.ShouldBeEmpty();
        }

        [Fact]
        public void When_EnvironmentVariableSingleSimpleConfigurationSupplied_Then_Contents_Returned()
        {
            // Arrange
            var configView = new Config.ConfigView("Key", "Key", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value1"
                });
            var expected = new List<Config.ConfigView> { configView };
            FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_EnvironmentVariableMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("InnerKey", "KeyRoot:InnerKey", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value1"
                }),
                new Config.ConfigView("OtherKey", "OtherKey", new Provider {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value2"
                })
             };
             FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_EnvironmentVariableMultipleMixedConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("Root", "Root", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value0"
                }),
                new Config.ConfigView("InnerKey", "KeyRoot:InnerKey", new Provider
                {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value1"
                }),
                new Config.ConfigView("OtherKey", "OtherKey", new Provider {
                    Source = "EnvironmentVariablesConfigurationProvider",
                    Value = "Value2"
                })
             };
             FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            // Order of the keys matters so use a default sorting order for comparison
            expected.OrderByDescending(e => e.Key)
                .ShouldBeEquivalentTo(actual.OrderByDescending(act => act.Key));
        }
    }
}