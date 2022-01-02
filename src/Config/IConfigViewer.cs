namespace ConfigView.Config
{
    internal interface IConfigViewer
    {
        IEnumerable<ConfigView> Get();
    }
}