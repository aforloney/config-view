using Xunit;
using ConfigView.Config.Viewers;
using ConfigView.Tests.Fakes;
using System.Linq;
using System.Collections.Generic;
using Shouldly;
using ConfigView.Config;

namespace ConfigView.Tests
{
    public class ConfigViewerTests
    {
        private IConfigViewer Subject { get; set; }
        private FakeContentProvider FakeContentProvider { get; }

        public ConfigViewerTests()
        {
            FakeContentProvider = new FakeContentProvider();
            Subject = new ConfigViewer(FakeContentProvider);
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
        public void When_MemoryConfigSimpleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
            var expected = new List<Config.ConfigView> {
                new Config.ConfigView("KeyA", "KeyA", new Provider
                {
                    Source = "MemoryConfigurationProvider",
                    Value = "ValueA"
                }),
             };
            FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_MemoryConfigMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
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
            FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_MemoryConfigAndJsonConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
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
            FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }
    }
}