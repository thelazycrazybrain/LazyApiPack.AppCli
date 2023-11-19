using System.Reflection;

namespace LazyApiPack.AppCli
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class FunctionNameAttribute : Attribute
    {
        private readonly string? _functionName;

        public FunctionNameAttribute() : this(null, true) { }
        public FunctionNameAttribute(string functionName, params string[] aliases) : this(functionName, false, aliases) { }

        public FunctionNameAttribute(string? functionName, bool isCaseSensitive = false, params string[] aliases)
        {
            _functionName = functionName;
            IsCaseSensitive = isCaseSensitive;
            Aliases = aliases ?? Array.Empty<string>();
        }

        public bool IsCaseSensitive { get; set; }
        public string[] Aliases { get; set; }
        public string? FunctionName { get => _functionName; }
    }

    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class FunctionParameterAttribute : Attribute
    {
        private readonly string? _parameterName;

        public FunctionParameterAttribute() : this(null, true, null) {}
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

    /// <summary>
    /// Use this attribute on a function parameter that is an IEnumerable&lt;string&gt; 
    /// to pass switch commands (parameters without value) to the function.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Parameter, Inherited = false , AllowMultiple = false)]
    sealed class FunctionSwitchesAttribute : Attribute
    {
        
        public FunctionSwitchesAttribute()
        {
        }

    }
    public class SampleApplication1
    {
        [FunctionName]
        public void Clear(
            [FunctionParameter] string a,
            [FunctionParameter] int b, 
            [FunctionParameter] int c,
            [FunctionSwitches] List<string> switches)
        {
            var n = new string[1];
            
        }

    }

    public class SampleApplication2
    {

    }

    public class ConsoleApplication
    {
        public void RegisterClass<TClass>()
        {
            var n = new SampleApplication1();
           
        }

        public void RegisterClass(object @class)
        {

        }
        public void UnregisterClass(object @class)
        {

        }

        public void UnregisterClass<TClass>()
        {

        }
        public object Execute(string command)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(string command)
        {
            throw new NotImplementedException();
        }
    }
}