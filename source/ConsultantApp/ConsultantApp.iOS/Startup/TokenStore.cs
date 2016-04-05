using System;
using Foundation;
using Newtonsoft.Json;
using Security;
using Shared.Core;
using Shared.Core.Platform;

namespace ConsultantApp.iOS.Startup
{
    public class TokenStore : ITokenStore
    {
        private const string ServiceName = "SiSystemsConsultantApp";
        private const string TokenLabel = "Certificate";
        private const string UsernameLabel = "Username";

        private static void CacheToken(string json)
        {
            CurrentUser.TokenCache = json;
        }

        public OAuthToken SaveToken(OAuthToken token)
        {
            var json = JsonConvert.SerializeObject(token);

            CacheToken(json);

            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = ServiceName,
                Label = TokenLabel,
            };
            var newRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = ServiceName,
                Label = TokenLabel,
                Account = token.Username,
                ValueData = NSData.FromString(json),
                Accessible = SecAccessible.AlwaysThisDeviceOnly
            };

            var addCode = SecKeyChain.Add(newRecord);
            if (addCode == SecStatusCode.DuplicateItem)
            {
                var remCode = SecKeyChain.Remove(existingRecord);
                if (remCode == SecStatusCode.Success)
                {
                    var addCode2 = SecKeyChain.Add(newRecord);
                }
            }

            return token;
        }

        private static OAuthToken TokenFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var token = JsonConvert.DeserializeObject<OAuthToken>(json);
            
            return token;
        }

        private string GetJsonFromCacheOrKeyChain()
        {
            return CurrentUser.TokenCache ?? GetJsonFromSecKeyChain();
        }

        private string GetJsonFromSecKeyChain()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = TokenLabel,
                Service = ServiceName
            };

            SecStatusCode resultCode;
            var data = SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            return resultCode == SecStatusCode.Success 
                ? NSString.FromData(data.ValueData, NSStringEncoding.UTF8) 
                : null;
        }

        public OAuthToken GetDeviceToken()
        {
            var json = GetJsonFromCacheOrKeyChain();

            var token = TokenFromJson(json);

            CurrentUser.Email = token.Username;

            return token;
        }

        private void DeleteTokenFromDevice()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = TokenLabel,
                Service = ServiceName
            };

            SecKeyChain.Remove(existingRecord);
        }

        private static void ClearTokenFromCache()
        {
            CurrentUser.TokenCache = null;
        }

        public void DeleteDeviceToken()
        {
            ClearTokenFromCache();
            DeleteTokenFromDevice();
        }

        public void SaveUserName(string username)
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = ServiceName,
                Label = UsernameLabel,
            };
            var newRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = ServiceName,
                Label = UsernameLabel,
                ValueData = NSData.FromString(username),
                Accessible = SecAccessible.AlwaysThisDeviceOnly
            };
            
            var addCode = SecKeyChain.Add(newRecord);
            if (addCode == SecStatusCode.DuplicateItem)
            {
                var remCode = SecKeyChain.Remove(existingRecord);
                if (remCode == SecStatusCode.Success)
                {
                    var addCode2 = SecKeyChain.Add(newRecord);
                }
            }
        }

        public string GetUserName()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = UsernameLabel,
                Service = ServiceName
            };

            SecStatusCode resultCode;
            var data = SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            if (resultCode == SecStatusCode.Success)
            {
               return NSString.FromData(data.ValueData, NSStringEncoding.UTF8);
            }
            return null;
        }
    }
}