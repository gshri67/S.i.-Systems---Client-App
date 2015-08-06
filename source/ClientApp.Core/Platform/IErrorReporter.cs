namespace Shared.Core.Platform
{
    public interface IErrorReporter
    {
        void Broadcast(string name, string message);

        void Display(string name, string message);
    }
}
