﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public static class Settings
    {
    #if LOCAL
        public const string MatchGuideApiAddress = "http://clientapi.local:50021/api/";
    #elif DEV
        public const string MatchGuideApiAddress = "https://clientapidev.azurewebsites.net/api/";
    #elif TEST
        public const string MatchGuideApiAddress = "https://clientapitest.azurewebsites.net/api/";
    #elif PROD
        public const string MatchGuideApiAddress = "https://clientapi.azurewebsites.net/api/";
    #endif
    }
}
