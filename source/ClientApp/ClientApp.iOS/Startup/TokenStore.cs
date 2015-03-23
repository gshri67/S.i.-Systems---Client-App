using Foundation;
using Newtonsoft.Json;
using Security;
using ClientApp.Core;
using ClientApp.Core.Platform;

namespace ClientApp.iOS.Startup
{
    public class TokenStore : ITokenStore
    {
        public OAuthToken SaveToken(OAuthToken token)
        {
            var json = JsonConvert.SerializeObject(token);
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = "SiSystemsClientApp",
                Label = "Certificate",
            };
            var newRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = "SiSystemsClientApp",
                Label = "Certificate",
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

        public OAuthToken GetDeviceToken()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = "Certificate",
                Service = "SiSystemsClientApp"
            };

            SecStatusCode resultCode;
            var data = SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            if (resultCode == SecStatusCode.Success)
            {
                var json = NSString.FromData(data.ValueData, NSStringEncoding.UTF8);
                var token = JsonConvert.DeserializeObject<OAuthToken>(json);
                CurrentUser.Email = token.Username;
                return token;
            }
            return null;
        }

        public void DeleteDeviceToken()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = "Certificate",
                Service = "SiSystemsClientApp"
            };

            SecKeyChain.Remove(existingRecord);
        }

        public void SaveUserName(string username)
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = "SiSystemsClientApp",
                Label = "Username",
            };
            var newRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = "SiSystemsClientApp",
                Label = "Username",
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
                Label = "Username",
                Service = "SiSystemsClientApp"
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