namespace LazyApiPack.AppCli
{
    public class InvalidCommandEventArgs
    {
        public InvalidCommandEventArgs(string command)
        {
            Command = command;
        }
        public string Command { get; private set; }
        public bool ContinueWithoutError { get; set; }
    }

}