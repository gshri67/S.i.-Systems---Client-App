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
        public ErrorSource(IErrorReporter reporter)
        {
            this.ErrorReported += (s, e) =>
            {
                if (e.Broadcast && !string.IsNullOrEmpty(e.Name))
                {
                    reporter.Broadcast(e.Name, e.Message);
                }
                if (!string.IsNullOrEmpty(e.Message))
                {
                    reporter.Display(e.Name, e.Message);
                }
            };
        }

        public void ReportError(string name, string message, bool broadcast = false)
        {
            if (ErrorReported == null)
                return;

            ErrorReported(this, new ErrorEventArgs(name, message, broadcast));
        }

        public event EventHandler<ErrorEventArgs> ErrorReported;
    }
}
