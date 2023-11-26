using System.Reflection;

namespace LazyApiPack.AppCli
{
    public class FunctionParameterDefinition
    {
        public FunctionParameterDefinition(ParameterInfo parameter, string? parameterName, bool isCaseSensitive, object? defaultValue, List<string>? aliases)
        {
            Parameter = parameter;
            ParameterName = parameterName;
            IsCaseSensitive = isCaseSensitive;
            DefaultValue = defaultValue;
            Aliases = aliases;
        }

        public ParameterInfo Parameter { get; private set; }
        public string? ParameterName { get; private set; }
        public bool IsCaseSensitive { get; private set; }
        public object? DefaultValue { get; private set; }
        public List<string>? Aliases { get; private set; }
        public static bool operator ==(FunctionParameterDefinition? param, string? paramName)
        {
            if (Equals(param, null) && paramName == null)
            {
                return true;
            }
            return string.Compare(param.ParameterName, paramName, !param.IsCaseSensitive) == 0 ||
                                  (param.Aliases?.Any(alias => string.Compare(alias, paramName, !param.IsCaseSensitive) == 0) ?? false);
        }

        public static bool operator !=(FunctionParameterDefinition param, string paramName)
        {
            if (Equals(param, null) ^ paramName == null)
            {
                return true;
            }
            return string.Compare(param.ParameterName, paramName, !param.IsCaseSensitive) != 0 &&
                                  (param.Aliases?.All(alias => string.Compare(alias, paramName, !param.IsCaseSensitive) != 0) ?? true);
        }
    }

}