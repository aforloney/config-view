using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config.Viewers
{
    internal interface IConfigViewer
    {
        IEnumerable<ConfigView> Get();
    }
}