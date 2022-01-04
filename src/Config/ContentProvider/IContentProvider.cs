namespace ConfigView.Config.ContentProvider
{
    internal interface IContentProvider
    {
        IEnumerable<ConfigView> Get(IEnumerable<Type> providerTypes);
    }
}