internal class ResponseBuffer
{
    private string buffer;
    private int delimiter = 0xFF;
    private int maxBufferSize = 100; // characters

    public ResponseBuffer()
    {
        buffer = "";
    }

    public bool DelimiterFound()
    {
        return buffer.Contains(delimiter);
    }

    public void Add(string response_fragment)
    {
        buffer += response_fragment;
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

    private CheckSize()
    {
        if (buffer.Length > maxBufferSize)
        {
            // TODO - how do I know I'm not throwing away a partial response?
        }
    }

}