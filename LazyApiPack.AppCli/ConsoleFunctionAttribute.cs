namespace LazyApiPack.AppCli
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ConsoleFunctionAttribute : Attribute
    {
        private readonly string? _functionName;

        public ConsoleFunctionAttribute() : this(null, true) { }
        public ConsoleFunctionAttribute(string functionName, params string[] aliases) : this(functionName, false, aliases) { }

        public ConsoleFunctionAttribute(string? functionName, bool isCaseSensitive = false, params string[] aliases)
        {
            _functionName = functionName;
            IsCaseSensitive = isCaseSensitive;
            Aliases = aliases ?? Array.Empty<string>();
        }

        public bool IsCaseSensitive { get; set; }
        public string[] Aliases { get; set; }
        public string? FunctionName { get => _functionName; }
    }

}