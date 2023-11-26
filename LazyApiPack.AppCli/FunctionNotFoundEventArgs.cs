namespace LazyApiPack.AppCli
{
    public class FunctionNotFoundEventArgs
    {
        public FunctionNotFoundEventArgs(string functionName, string command)
        {
            FunctionName = functionName;
            Command = command;
        }
        public string FunctionName { get; private set; }
        public string Command { get; private set; }
        public bool ContinueWithoutError { get; set; }
    }

}