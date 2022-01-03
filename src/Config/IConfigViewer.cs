using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConfigView.Tests")]
namespace ConfigView.Config
{
    internal interface IConfigViewer
    {
        IEnumerable<ConfigView> Get();
    }
}