namespace VISCACommandSet.Utilities
{
    public class ResponseBuffer
    {
        private StringBuilder buffer;
        private char delimiter = '\xFF';
        private int maxBufferSize = 1000;
        private static Mutex mutex = new Mutex();

        public ResponseBuffer()
        {
            buffer = new StringBuilder();
        }

        public void Add(string responseFragment)
        {
            mutex.WaitOne();
            try
            {
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

            // mutex is used to prevent the buffer from being modified while we are iterating through it
            mutex.WaitOne();
            try
            {
                int index = 0;
                while (index < buffer.Length)
                {
                    if (buffer[index] == delimiter)
                    {
                        // Extract the response including the delimiter
                        string response = buffer.ToString(0, index + 1);
                        responses.Add(response);

                        // Remove the extracted response from the buffer
                        buffer.Remove(0, index + 1);

                        // Reset the index to start from the beginning of the updated buffer
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

        private void EmptyBuffer()
        {
            mutex.WaitOne();
            try
            {
                buffer.Clear();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private void CheckSize()
        {
            mutex.WaitOne();
            try
            {
                if (buffer.Length > maxBufferSize)
                {
                    EmptyBuffer();
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
