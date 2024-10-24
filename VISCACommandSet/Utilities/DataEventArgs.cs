using System;

namespace VISCACommandSet.Utilities
{
    public class DataEventArgs : EventArgs
    {
        public string CompleteMessage { get; }

        public DataEventArgs(string message)
        {
            CompleteMessage = message;
        }
    }
}