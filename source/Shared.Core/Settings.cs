namespace Shared.Core
{
    public static class Settings
    {
        public const string DemoDomain = "@siAKSdemo.com";
        public const string DemoMatchGuideApiAddress = "https://clientapitest.azurewebsites.net/api/";
    #if LOCAL
        //public const string MatchGuideApiAddress = "http://clientapi.local:50021/api/";
                public const string MatchGuideApiAddress = "https://clientapidev.azurewebsites.net/api/";
#elif DEV
        public const string MatchGuideApiAddress = "https://clientapidev.azurewebsites.net/api/";
    #elif TEST
        public const string MatchGuideApiAddress = "https://clientapidev.azurewebsites.net/api/";
        //public const string MatchGuideApiAddress = "https://clientapitest.azurewebsites.net/api/";
        //public const string MatchGuideApiAddress = "https://clientapi.azurewebsites.net/api/";
    #elif PROD
        public const string MatchGuideApiAddress = "https://clientapi.azurewebsites.net/api/";
#endif
    }
}
