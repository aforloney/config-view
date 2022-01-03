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

            // Act
            var config = Subject.Get();

            // Assert
            Assert.Equal(config, Enumerable.Empty<Config.ConfigView>());
        }

        [Fact]
        public void When_JsonStreamConfigurationSupplied_Then_Contents_Returned()
        {
            // Arrange
            var jsonString = @"{""Key"":""Value2""}";
            var rootConfig = FakeConfiguration.ConfigureRoot(jsonString);
            Subject = new ConfigViewer(rootConfig);
            var configView = new Config.ConfigView("Key", "Key", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
                    Value = "Value2"
                });
            var expected = new List<Config.ConfigView> { configView };

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }
    }
}