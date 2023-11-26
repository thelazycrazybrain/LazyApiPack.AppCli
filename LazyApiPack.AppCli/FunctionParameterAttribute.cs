namespace LazyApiPack.AppCli
{
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FunctionParameterAttribute : Attribute
    {
        private readonly string? _parameterName;

        public FunctionParameterAttribute() : this(null, true, null) { }
        public FunctionParameterAttribute(string? parameterName, bool isCaseSensitive, object? defaultValue) : this(parameterName, isCaseSensitive, defaultValue, Array.Empty<string>()) { }

        public FunctionParameterAttribute(string? parameterName, bool isCaseSensitive, object? defaultValue, params string[] aliases)
        {
            _parameterName = parameterName;
            IsCaseSensitive = isCaseSensitive;
            DefaultValue = defaultValue;
            Aliases = aliases ?? Array.Empty<string>();
        }
        public string? ParameterName { get => _parameterName; }
        public bool IsCaseSensitive { get; set; }
        public object? DefaultValue { get; set; }
        public string[] Aliases { get; set; }
    }

}