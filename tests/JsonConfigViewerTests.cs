using Xunit;
using ConfigView.Config;
using ConfigView.Tests.Fakes;
using System.Linq;
using System.Collections.Generic;
using Shouldly;

namespace ConfigView.Tests
{
    public class JsonConfigViewerTests
    {
        private IConfigViewer Subject { get; set; }
        private FakeContentProvider FakeContentProvider { get; }

        public JsonConfigViewerTests()
        {
            FakeContentProvider = new FakeContentProvider();
            Subject = new JsonConfigViewer(FakeContentProvider);
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
        public void When_NonJsonStreamConfigurationSupplied_Then_Empty()
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
        public void When_JsonStreamSingleSimpleConfigurationSupplied_Then_Contents_Returned()
        {
            // Arrange
            var configView = new Config.ConfigView("Key", "Key", new Provider
                {
                    Source = "JsonStreamConfigurationProvider",
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
        public void When_JsonStreamMultipleConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
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
             FakeContentProvider.SetConfigs(expected);

            // Act
            var actual = Subject.Get();

            // Assert
            expected.ShouldBeEquivalentTo(actual);
        }

        [Fact]
        public void When_JsonStreamMultipleMixedConfigurationsSupplied_Then_Contents_Returned()
        {
            // Arrange
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