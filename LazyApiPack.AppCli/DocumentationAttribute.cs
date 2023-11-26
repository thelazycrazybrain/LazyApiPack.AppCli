namespace LazyApiPack.AppCli
{
    [System.AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class DocumentationAttribute : Attribute
    {
        readonly string _documentation;

        public DocumentationAttribute(string documentation)
        {
            _documentation = documentation;
        }

        public string Documentation => _documentation;

    }

}