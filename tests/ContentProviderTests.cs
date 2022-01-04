using Xunit;
using ConfigView.Config;
using ConfigView.Tests.Fakes;
using System.Linq;
using System.Collections.Generic;
using Shouldly;
using ConfigView.Config.ContentProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.Memory;

namespace ConfigView.Tests
{
    public class ContentProviderTests
    {
        private IContentProvider Subject { get; set; }
        private FakeConfiguration FakeConfiguration { get; }

        public ContentProviderTests()
        {
            FakeConfiguration = new FakeConfiguration();
            Subject = new ContentProvider(FakeConfiguration);
        }

        [Fact]
        public void When_NoConfigurationAllRequested_Then_Empty()
        {
            // Arrange
            // intetionally blank
            // Act
            var config = Subject.Get(new [] { typeof(IConfigurationProvider) });

            // Assert
            Assert.Equal(config, Enumerable.Empty<Config.ConfigView>());
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
        public void When_MemoryConfigAndJsonConfigurationsSuppliedAllRequested_Then_Contents_Returned()
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
    }
}