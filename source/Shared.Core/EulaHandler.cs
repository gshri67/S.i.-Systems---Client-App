using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SiSystems.SharedModels;

namespace Shared.Core
{
    public static class EulaHandler
    {
        public static Eula Eula { get; set; }
        public static Dictionary<string, int> EulaVersions { get; private set; }
        
        public static bool UserHasReadLatestEula(string username, int version, string storageString)
        {
            try
            {
                EulaVersions = JsonConvert.DeserializeObject<Dictionary<string, int>>(storageString);
            }
            catch (Exception)
            {
                //TODO log error
                EulaVersions = new Dictionary<string, int>();
                return false;
            }

            if (EulaVersions.ContainsKey(username))
            {
                if (EulaVersions[username] == version)
                {
                    return true;
                }
            }
            return false;
        }
    }
}