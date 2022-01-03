using Xunit;
using ConfigView.Config;
using ConfigView.Tests.Fakes;
using System.Linq;
using System.Collections.Generic;
using Shouldly;

namespace ConfigView.Tests
{
    public class ConfigViewerTests
    {
        private IConfigViewer Subject { get; set; }
        private FakeConfiguration FakeConfiguration { get; }

        public ConfigViewerTests()
        {
            FakeConfiguration = new FakeConfiguration();
            Subject = new ConfigViewer(FakeConfiguration);
        }

        [Fact]
        public void When_NoConfiguration_Then_Empty()
        {
            // Arrange
            // intetionally blank
            // Act
            var config = Subject.Get();

            // Assert
            Assert.Equal(config, Enumerable.Empty<Config.ConfigView>());
        }

        [Fact]
        public void When_JsonStreamSingleSimpleConfigurationSupplied_Then_Contents_Returned()
        {
            // Arrange
            var jsonString = @"{""Key"":""Value1""}";
            Subject = new ConfigViewer(FakeConfiguration.ConfigureRoot(jsonString));
            var configView = new Config.ConfigView("Key", "Key", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value1"
                });
            var expected = new List<Config.ConfigView> { configView };

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_JsonStreamSingleNestedConfigurationSupplied_Then_Contents_Returned()
        {
            // Arrange
            var jsonString = @"{""KeyRoot"":{""InnerKey"":""Value1""}}";
            Subject = new ConfigViewer(FakeConfiguration.ConfigureRoot(jsonString));
            var configView = new Config.ConfigView("InnerKey", "KeyRoot:InnerKey", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value1"
                });
            var expected = new List<Config.ConfigView> { configView };

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_JsonStreamMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            var jsonString = @"{""KeyRoot"":{""InnerKey"":""Value1""},""OtherKey"":""Value2""}";
            Subject = new ConfigViewer(FakeConfiguration.ConfigureRoot(jsonString));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("InnerKey", "KeyRoot:InnerKey", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value1"
                }),
                new Config.ConfigView("OtherKey", "OtherKey", new Provider {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value2"
                })
             };

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_JsonStreamMultipleMixedConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            var jsonString = @"{""Root"":""Value0"",""KeyRoot"":{""InnerKey"":""Value1""},""OtherKey"":""Value2""}";
            Subject = new ConfigViewer(FakeConfiguration.ConfigureRoot(jsonString));
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("Root", "Root", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value0"
                }),
                new Config.ConfigView("InnerKey", "KeyRoot:InnerKey", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value1"
                }),
                new Config.ConfigView("OtherKey", "OtherKey", new Provider {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value2"
                })
             };

            // Act
            var actual = Subject.Get();

            // Assert
            // Order of the keys matters so use a default sorting order for comparison
            expected.OrderByDescending(e => e.Key)
                .ShouldBeEquivalentTo(actual.OrderByDescending(act => act.Key));
        }
    }
}