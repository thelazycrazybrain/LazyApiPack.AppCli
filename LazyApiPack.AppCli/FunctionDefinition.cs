using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.AppCli
{
    public class FunctionDefinition
    {
        public FunctionDefinition([NotNull] MethodInfo function, string? functionName, bool isCaseSensitive,
            List<string>? aliases, List<FunctionParameterDefinition>? parameters)
        {
            Function = function;
            FunctionName = functionName;
            IsCaseSensitive = isCaseSensitive;
            Aliases = aliases;
            Parameters = parameters;

        }

        public MethodInfo Function { get; private set; }
        public string? FunctionName { get; private set; }
        public bool IsCaseSensitive { get; private set; }
        public List<string>? Aliases { get; private set; }
        public List<FunctionParameterDefinition>? Parameters { get; private set; }


        public static bool operator ==(FunctionDefinition? func, string? funcName)
        {
            if (Equals(func, null) && funcName == null)
            {
                return true;
            }

            return string.Compare(func.FunctionName, funcName, !func.IsCaseSensitive) == 0 ||
                                  (func.Aliases?.Any(alias => string.Compare(alias, funcName, !func.IsCaseSensitive) == 0) ?? false);
        }

        public static bool operator !=(FunctionDefinition? func, string? funcName)
        {
            if (Equals(func, null) ^ funcName == null)
            {
                return true;
            }
            return string.Compare(func.FunctionName, funcName, !func.IsCaseSensitive) != 0 &&
                                  (func.Aliases?.All(alias => string.Compare(alias, funcName, !func.IsCaseSensitive) != 0) ?? true);
        }

    }

}