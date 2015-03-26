using ClientApp.Core.Platform;
using System;

namespace ClientApp.Core
{
    public class ErrorEventArgs : EventArgs
    {
        

        public string Name { get; private set; }

        public string Message { get; private set; }

        public bool Broadcast { get; private set; }

        public ErrorEventArgs(string name, string message, bool broadcast)
        {
            Name = name;
            Message = message;
            Broadcast = broadcast;
        }
    }

    public interface IErrorSource
    {
        void ReportError(string name, string message, bool broadcast = false);

        event EventHandler<ErrorEventArgs> ErrorReported;
    }

    public class ErrorSource : IErrorSource
    {
        private static Tuple<DateTime, string, string> _lastEvent;

        private static TimeSpan _debounce = TimeSpan.FromSeconds(0.5);

        public event EventHandler<ErrorEventArgs> ErrorReported;

        public void ReportError(string name, string message, bool broadcast = false)
        {
            if (ErrorReported == null)
                return;

            // prevent an error from being reported multiple times in quick succession
            if (_lastEvent != null && DateTime.Now.Subtract(_lastEvent.Item1) <= _debounce && _lastEvent.Item2 == name && _lastEvent.Item3 == message)
                return;

            _lastEvent = new Tuple<DateTime, string, string>(DateTime.Now, name, message); 

            ErrorReported(this, new ErrorEventArgs(name, message, broadcast));
        }
    }
}
