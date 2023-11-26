namespace LazyApiPack.AppCli
{
    [Serializable]
    public class InvalidCommandException : Exception
    {
        private const string INVALIDCMD = "Command '{0}' is invalid.";
        public InvalidCommandException(string command) : base(string.Format(INVALIDCMD, command)) { }
        public InvalidCommandException(string command, string message) : base($"{string.Format(INVALIDCMD, command)}\r\n{message}") { }
        public InvalidCommandException(string command, string message, Exception inner) : base($"{string.Format(INVALIDCMD, command)}\r\n{message}", inner) { }
        protected InvalidCommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}