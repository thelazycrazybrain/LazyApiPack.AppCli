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
        public bool IsRunning = true;
        [ConsoleFunction]
        [Documentation("Formats a text with the given parameters.")]
        public string Format(
            [FunctionParameter]
                [Documentation("The format string indexed with {i} 0-n")]
                string text,
            [FunctionParameter(DefaultValue = false)]
                [Documentation("If set, the resulting string is in upper case.")]
                bool upperCase,
            [FunctionParameter]
                [Documentation("The list of parameters used in the text.")]
                params string[] args)
        {
            var format = string.Format(text, args);
            return upperCase ? format.ToUpper() : format;
        }

        [ConsoleFunction]
        [Documentation("Returns \"Hello World\"")]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [ConsoleFunction("exit", "quit", "xit", "close")]
        [Documentation("Closes the application.")]
        public void Exit()
        {
            IsRunning = false;
        }

        protected override void OnFunctionNotFound(FunctionNotFoundEventArgs e)
        {
            Console.WriteLine($"Command {e.FunctionName} not found.");
            e.ContinueWithoutError = true;
        }
        protected override void OnExecuted(object? result)
        {
            Console.WriteLine();
            Console.Write("> ");
        }

        protected override void OnInvalidCommand(InvalidCommandEventArgs e)
        {
            Console.WriteLine($"Command '{e.Command}' is invalid.");
            e.ContinueWithoutError = true;
        }

    }
}
