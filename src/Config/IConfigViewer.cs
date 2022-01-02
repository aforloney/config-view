namespace Confv.Config 
{
    internal interface IConfigViewer
    {
        IEnumerable<ConfigView> Get();
    }
}