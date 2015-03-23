namespace ClientApp.Core.Platform
{
    public interface ITokenStore
    {
        OAuthToken SaveToken(OAuthToken token);

        OAuthToken GetDeviceToken();

        void SaveUserName(string username);

        string GetUserName();

        void DeleteDeviceToken();
    }
}
