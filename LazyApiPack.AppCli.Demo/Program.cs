using LazyApiPack.AppCli;

internal class Program
{
    private static void Main(string[] args)
    {
        var app = new ConsoleApp1();
        Console.WriteLine("**********************************************");
        Console.WriteLine("* LazyApiPack Command Line Interface Program *");
        Console.WriteLine("**********************************************");
        Console.WriteLine();
        Console.Write("> ");
        while (app.IsRunning)
        {
            var @in = Console.ReadLine();
            if (@in == null)
            {
                continue;
            }
            app.Execute(@in);
        }
    }

    public class ConsoleApp1 : ConsoleApplication
    {
        [ConsoleFunction]
        [Documentation("Formats a text with the given parameter.")]
        public string Format(
            [FunctionParameter]
                [Documentation("The format string indexed with {i} 0-n")]
                string text,
            [FunctionParameter(DefaultValue = false)]
                [Documentation("If set, the resulting string is in upper case.")]
                bool upperCase,
            [FunctionParameter]
                [Documentation("The list of parameters used in the text.")]
                string arg)
        {
            var format = string.Format(text, arg);
            return upperCase ? format.ToUpper() : format;
        }

        [ConsoleFunction]
        public void PrintParams(params string[] args)
        {
            PrintLine("Print Params: ");
            foreach (var param in args)
            {
                PrintLine(param);
            }
        }
        [ConsoleFunction]
        [Documentation("Returns \"Hello World\"")]
        public string HelloWorld()
        {
            return "Hello World";
        }

        protected override void OnFunctionNotFound(FunctionNotFoundEventArgs e)
        {
            PrintLine($"Command {e.FunctionName} not found.");
            e.ContinueWithoutError = true;
        }
        protected override void OnExecuted(object? result)
        {
            PrintLine();
            Print("> ");
        }

        protected override void OnInvalidCommand(InvalidCommandEventArgs e)
        {
            PrintLine($"Command '{e.Command}' is invalid.");
            e.ContinueWithoutError = true;
        }

    }
}
