namespace LazyApiPack.AppCli
{
    [Serializable]
    public class FunctionNotFoundException : Exception
    {
        private const string FUNCNOTFOUND = "Function '{0}' was not found.";
        public FunctionNotFoundException(string functionName) : base(string.Format(FUNCNOTFOUND, functionName)) { }
        public FunctionNotFoundException(string functionName, string message) : base($"{string.Format(FUNCNOTFOUND, functionName)}\r\n{message}") { }
        public FunctionNotFoundException(string functionName, string message, Exception inner) : base($"{string.Format(FUNCNOTFOUND, functionName)}\r\n{message}", inner) { }
        protected FunctionNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}