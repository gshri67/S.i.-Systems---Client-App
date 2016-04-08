namespace Shared.Core.Platform
{
    public interface ITokenStore
    {
        bool SaveToken(string username, string token);

        string GetDeviceToken();

        void DeleteDeviceToken();
    }
}
