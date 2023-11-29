using System;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace LazyApiPack.AppCli
{
    public abstract class ConsoleApplication
    {

        private List<FunctionDefinition> _functions = new();
        public ConsoleApplication()
        {
            var functions = this
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute<ConsoleFunctionAttribute>() != null);
            foreach (var function in functions)
            {
                var parameters = new List<FunctionParameterDefinition>();
                foreach (var param in function.GetParameters())
                {
                    var pAtt = param.GetCustomAttribute<FunctionParameterAttribute>();
                    parameters.Add(new FunctionParameterDefinition(param,
                        pAtt?.ParameterName ?? param.Name,
                        pAtt?.IsCaseSensitive ?? false,
                        pAtt?.DefaultValue ?? (param.ParameterType.IsClass ? null : Activator.CreateInstance(param.ParameterType)),
                        pAtt?.Aliases?.ToList()));

                }

                var fAtt = function.GetCustomAttribute<ConsoleFunctionAttribute>();

                _functions.Add(new FunctionDefinition(function,
                    !string.IsNullOrWhiteSpace(fAtt.FunctionName) ? fAtt.FunctionName : function.Name,
                    fAtt.IsCaseSensitive, fAtt.Aliases?.ToList(), parameters));

            }
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="command">The console input of the user.</param>
        /// <returns>A result, produced by a command (NULL if void).</returns>
        public object? Execute([DisallowNull] string command)
        {
            command = OnExecute(command);
            object? result = null;
            //try
            //{
            var paramRegex = new Regex(@"(?:""(?:[^""\\]|\\.)*""|\S+)");
            string cmdName = null;
            var @params = new List<string>();

            foreach (Match match in paramRegex.Matches(command))
            {

                if (match.Success && !string.IsNullOrEmpty(match.Value))
                {
                    if (cmdName == null)
                    {
                        cmdName = match.Value;
                    }
                    else
                    {
                        @params.Add(match.Value);
                    }
                }
            }

            if (cmdName == null)
            {
                InvalidCommand(command);

            }
            var cmd = _functions.FirstOrDefault(f => f == cmdName);
            if (cmd == null)
            {
                var e = new FunctionNotFoundEventArgs(cmdName, command);
                OnFunctionNotFound(e);
                if (e.ContinueWithoutError)
                {
                    return null;
                }
                else
                {
                    throw new FunctionNotFoundException(cmdName);
                }
            }


            //object[] callingParameters = new[] 
            //foreach (var param in ;)
            //{
            //    if()
            //}

            // Only pass list of parameters to function if
            // Func(params string[] parameters)
            if (cmd.Parameters.Count == 1 && cmd.Parameters[0].Parameter.ParameterType.IsAssignableTo(typeof(IEnumerable<string>)))
            {
                result = cmd.Function.Invoke(this, new[] { @params.ToArray() });
            }
            else
            {
                // More complex function
                var parameters = GetParameters(@params, cmd.Parameters);
                if (parameters.Count() == 0)
                {
                    result = cmd.Function.Invoke(this, null);
                }
                else
                {
                    result = cmd.Function.Invoke(this, parameters.Select(v => v.Value).ToArray());
                }
            }
            return result;
            //}
            //finally
            //{
            //    OnExecuted(result);
            //}

        }
        private class ParsedParameter
        {
            public string? Identifier { get; set; }
            public bool CaseSensitive { get; set; }
            public bool HasValue { get; set; }
            public object? Value { get; set; }
        }

        private IEnumerable<ParsedParameter> GetParameters(IEnumerable<string> consoleParameters, IEnumerable<FunctionParameterDefinition>? definedParameters)
        {
            var indexableParameters = consoleParameters.ToArray();
            var result = new List<ParsedParameter>();
            if (definedParameters == null || !definedParameters.Any() || !consoleParameters.Any())
            {
                return result; // no params given or required.
            }
            //CommanderFunctionParameterDefinition mp = null;
            //if (indexableParameters.Length == 0 && (mp = definedParameters.FirstOrDefault(p => !p.IsOptional)) != null) {
            //    throw new ArgumentException(LocalizationHelper.GetFormattedLocalizedText("Commander.Errors.MandatoryParameterMissing", "The mandatory parameter {0} is missing", mp.Identifier));
            //}

            foreach (var parameter in definedParameters)
            {
                var currentParameter = new ParsedParameter();

                //var index = 0;
                var strValue = string.Empty;
                if (indexableParameters.Length == 1)
                {
                    strValue = indexableParameters[0];
                }
                for (int i = 0; i < indexableParameters.Length; i++)
                {
                    var id = indexableParameters[i].TrimStart().TrimStart('-').Trim();

                    if (indexableParameters[i].Trim().StartsWith("-") &&
                        parameter.Parameter.ParameterType.IsGenericParameter || parameter == id)
                    {
                        // If the parameter is an identifier
                        // Either its generic or the identifier matches or any of its aliases matches

                        currentParameter.Identifier = indexableParameters[i].TrimStart().TrimStart('-').Trim();
                        currentParameter.CaseSensitive = parameter.IsCaseSensitive;
                        if (!parameter.Parameter.ParameterType.IsAssignableTo(typeof(bool)))
                        {
                            strValue = indexableParameters[++i].Trim();

                        }
                        break;
                    }
                }

                if (typeof(bool).IsAssignableFrom(parameter.Parameter.ParameterType) || typeof(Boolean).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    if (string.Compare(strValue, "true", true) == 0)
                    {
                        currentParameter.Value = true;
                    }
                    else if (string.Compare(strValue, "false", true) == 0)
                    {
                        currentParameter.Value = false;
                    }
                    else
                    {
                        currentParameter.Value = true; // Flag!
                    }
                }
                if (typeof(string).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = strValue?.Trim('\"');
                }
                else if (typeof(byte).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = byte.Parse(strValue);
                }
                else if (typeof(short).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = short.Parse(strValue);
                }
                else if (typeof(int).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = int.Parse(strValue);
                }
                else if (typeof(long).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = long.Parse(strValue);
                }
                else if (typeof(float).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = float.Parse(strValue);
                }
                else if (typeof(double).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = double.Parse(strValue);
                }
                else if (typeof(decimal).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = decimal.Parse(strValue);
                }
                else if (typeof(Version).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = Version.Parse(strValue?.Trim('\"'));
                }
                else if (typeof(Guid).IsAssignableFrom(parameter.Parameter.ParameterType))
                {
                    currentParameter.Value = Guid.Parse(strValue?.Trim('\"'));
                }
                else
                {
                    throw new NotSupportedException("This type is not known.");
                }

                result.Add(currentParameter);

            }
            return result;
        }

        private void InvalidCommand(string command)
        {
            var e = new InvalidCommandEventArgs(command);
            OnInvalidCommand(e);
            if (e.ContinueWithoutError)
            {
                return;
            }
            else
            {
                throw new InvalidCommandException(command);
            }
        }

        [ConsoleFunction("Man", "man", "manual", "help", "?")]
        [Documentation("Shows the documentation to a function")]
        public string Man(
            [FunctionParameter]
            [Documentation("The name of the function.")]
            string functionName)
        {
            var sb = new StringBuilder();
            var func = _functions.FirstOrDefault(f => f == functionName);
            if (func == null)
            {
                sb.AppendLine($"Function {functionName} is not known.");
            }
            else
            {
                var dAtt = func.Function.GetCustomAttribute<DocumentationAttribute>();
                if (dAtt == null)
                {
                    GetFunctionDocumentation(sb, func);
                    sb.AppendLine("Parameters:");
                    foreach (var param in func.Parameters)
                    {
                        GetParameterDocumentation(sb, param);
                    }
                }
                else
                {
                    GetFunctionDocumentation(sb, func);
                    sb.AppendLine($"\t{dAtt.Documentation}");

                    sb.AppendLine("Parameters:");
                    foreach (var param in func.Parameters)
                    {
                        GetParameterDocumentation(sb, param);
                        var pAtt = param.Parameter.GetCustomAttribute<DocumentationAttribute>();
                        if (pAtt != null)
                        {
                            sb.AppendLine($"\t{pAtt.Documentation}");
                        }

                    }
                }
            }
            Print(sb.ToString());
            return sb.ToString();
        }

        private void GetParameterDocumentation(StringBuilder sb, FunctionParameterDefinition param)
        {
            sb.AppendLine($"\t-{param.ParameterName}");
            if (param.Aliases?.Any() ?? false)
            {
                sb.AppendLine("\tAliases:");
                foreach (var alias in param.Aliases)
                {
                    sb.Append($" -{alias}");
                }
            }
        }

        private void GetFunctionDocumentation(StringBuilder sb, FunctionDefinition func)
        {
            sb.AppendLine(func.FunctionName);
            if (func.Aliases?.Any() ?? false)
            {
                sb.Append("\tAliases:");
                foreach (var alias in func.Aliases)
                {
                    sb.Append($" {alias}");
                }
                sb.AppendLine();
            }

        }

        public bool IsRunning { get; protected set; } = true;

        [ConsoleFunction("exit", "quit", "xit", "close")]
        [Documentation("Closes the application.")]
        public virtual void Exit()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the command.</typeparam>
        /// <param name="command">The console input of the user.</param>
        /// <returns>A result, produced by a command (NULL if void).</returns>
        public TResult Execute<TResult>(string command)
        {
            return (TResult)Execute(command);
        }

        protected virtual void OnPrint(string? text)
        {
            Console.Write(text);
        }

        /// <summary>
        /// Prints a line.
        /// </summary>
        protected void PrintLine(string? text = null)
        {
            OnPrint(text + "\r\n");
        }

        /// <summary>
        /// Prints text without a linebreak.
        /// </summary>
        protected void Print(string? text = null)
        {
            OnPrint(text);
        }

        protected virtual void OnInvalidCommand(InvalidCommandEventArgs e)
        {

        }
        protected virtual void OnFunctionNotFound(FunctionNotFoundEventArgs e)
        {

        }
        /// <summary>
        /// Is called after a command has executed.
        /// </summary>
        /// <param name="result">The result of the executed command.</param>
        protected virtual void OnExecuted(object? result)
        {

        }
        /// <summary>
        /// Method to modify or debug the entered command before execution.
        /// </summary>
        /// <param name="command">Command entered by the user.</param>
        /// <returns>Modified command.</returns>
        protected virtual string OnExecute(string command)
        {
            return command;
        }



    }

}