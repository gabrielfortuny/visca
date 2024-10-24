namespace VISCACommandSet.Utilities
{
    public class DataEventArgs : EventArgs
    {
        public string Data { get; }

        public DataEventArgs(string data)
        {
            this.Data = data;
        }

        
    }
}
