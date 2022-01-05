namespace ConfigView.Config.Viewers
{
    internal interface IConfigViewer
    {
        IEnumerable<ConfigView> Get();
    }
}