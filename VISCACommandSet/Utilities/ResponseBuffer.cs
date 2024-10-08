namespace VISCACommandSet.Utilities
{
    public class ResponseBuffer
    {
        private StringBuilder buffer = new StringBuilder();
        private readonly char delimiter;
        private readonly int maxBufferSize;
        // a mutex is necessary to prevent multiple threads from modifying the buffer at the same time
        private static readonly Mutex mutex = new Mutex();

        public ResponseBuffer(char delimiter, int maxBufferSize)
        {
            this.delimiter = delimiter;
            this.maxBufferSize = maxBufferSize;
        }

        public void Add(string responseFragment)
        {
            mutex.WaitOne();
            try
            {
                // empties the buffer if this addition will go over maxBufferSize
                CheckBufferOverflow(responseFragment.Length);
                buffer.Append(responseFragment);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public List<string> ExtractResponses()
        {
            // there may be multiple responses in the buffer, so we will iterate through the buffer and remove them as we find them
            List<string> responses = new List<string>();

            mutex.WaitOne();
            try
            {
                int index = 0;
                while (index < buffer.Length)
                {
                    if (buffer[index] == delimiter)
                    {
                        // extract the response including the delimiter
                        string response = buffer.ToString(0, index + 1);
                        responses.Add(response);

                        // Remove the extracted response from the buffer
                        buffer.Remove(0, index + 1);

                        // reset the index to start from the beginning of the updated buffer
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return responses;
        }

        private void CheckBufferOverflow(int fragmentLength)
        {
            mutex.WaitOne();
            try
            {
                if (buffer.Length + fragmentLength > maxBufferSize)
                {
                    buffer.Clear();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
