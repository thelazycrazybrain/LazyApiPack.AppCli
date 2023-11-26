namespace LazyApiPack.AppCli.Tests
{
    public class ConsoleApplicationTests
    {
        ConsoleApp1 _app;
        [SetUp]
        public void Setup()
        {
            _app = new ConsoleApp1();
        }

        [Test]
        public void TestExecute()
        {
            //var result = _app.Execute("HelloWorld");
            //Assert.IsTrue(result == "Hello World");

            var result = _app.Execute<string>("Format -upperCase \"Hello \\\"{0} {1}\\\" World!\" \"you\" \"beautiful\"");
            Assert.IsTrue(result == "HELLO YOU BEAUTIFUL WORLD!");
        }

        [Test]
        public void TestFunctionDefinitionOperators()
        {
            var fd = new FunctionDefinition(null, "Test", false, new[] { "tets", "tset" }.ToList(), null);
            var @in = "TEST";
            Assert.True(fd == @in, "== is not true (case insensitive).");
            Assert.False(fd != @in, "!= is not false (case insensitive).");

            fd = new FunctionDefinition(null, "Test", true, new[] { "tets", "tset" }.ToList(), null);
            Assert.False(fd == @in, "== is not false (case sensitive).");
            @in = "tets";
            Assert.True(fd == @in, "== is not true (case sensitive alias).");
            @in = "Tets";
            Assert.False(fd == @in, "== is not false (case sensitive alias).");
            @in = null;
            Assert.False(fd == @in, "== Null check failed when null.");
            Assert.True(fd != @in, "!= Null check failed when null.");
            fd = new FunctionDefinition(null, null, true, new[] { "tets", "tset" }.ToList(), null);
            Assert.True(fd == @in, "== Null check failed when not null.");
            Assert.True(fd != @in, "!= Null check failed when not null.");
            Assert.Pass();
        }

        [Test]
        public void TestFunctionParameterOperators()
        {
            var pd = new FunctionParameterDefinition(null, "Test", false, null, new[] { "tets", "tset" }.ToList());
            var @in = "TEST";
            Assert.True(pd == @in, "== is not true (case insensitive).");
            Assert.False(pd != @in, "!= is not false (case insensitive).");

            pd = new FunctionParameterDefinition(null, "Test", true, null, new[] { "tets", "tset" }.ToList());
            Assert.False(pd == @in, "== is not false (case sensitive).");
            @in = "tets";
            Assert.True(pd == @in, "== is not true (case sensitive alias).");
            @in = "Tets";
            Assert.False(pd == @in, "== is not false (case sensitive alias).");
            @in = null;
            Assert.False(pd == @in, "== Null check failed when null.");
            Assert.True(pd != @in, "!= Null check failed when null.");
            pd = new FunctionParameterDefinition(null, null, true, null, new[] { "tets", "tset" }.ToList());
            Assert.True(pd == @in, "== Null check failed when not null.");
            Assert.True(pd != @in, "!= Null check failed when not null.");
            Assert.Pass();
        }
        public class ConsoleApp1 : ConsoleApplication
        {

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

            private bool _isExecuting = false;
            protected override string OnExecute(string command)
            {
                Assert.IsFalse(_isExecuting, "App is already executing a command but OnExecute was called.");
                _isExecuting = true;
                return command;
            }

            protected override void OnFunctionNotFound(FunctionNotFoundEventArgs e)
            {
                Assert.Fail($"Function '{e.FunctionName}' with command '{e.Command}' was not found.");
            }
            protected override void OnExecuted(object? result)
            {
                Assert.IsTrue(_isExecuting, "App is not executing but OnExecuted was called.");
                _isExecuting = false;
            }

            protected override void OnInvalidCommand(InvalidCommandEventArgs e)
            {
                Assert.Fail($"Command '{e.Command}' is invalid.");
            }
        }




    }


}