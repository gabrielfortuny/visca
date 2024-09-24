namespace VISCACommandSet.Utilities
{
    public class ResponseBuffer
    {
        private string buffer;
        private char delimiter = '\xFF';
        private int maxBufferSize = 1000; // characters

        public ResponseBuffer()
        {
            buffer = "";
        }

        public bool DelimiterFound()
        {
            return buffer.Contains(delimiter);
        }

        public string Add(string response_fragment)
        {
            buffer += response_fragment;
            return "TODO";
        }

        private void EmptyBuffer()
        {
            buffer = "";
        }

        public string ExtractResponse()
        {
            string response = buffer.Substring(0, buffer.IndexOf(delimiter));
            EmptyBuffer();
            return response;
        }

        private void CheckSize()
        {
            if (buffer.Length > maxBufferSize)
            {
                EmptyBuffer();
            }
        }
    }
}
