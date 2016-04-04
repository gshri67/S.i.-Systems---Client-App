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

        public OAuthToken SaveToken(OAuthToken token)
        {
#if TEST
            Console.WriteLine("SaveToken Start");
#endif
            var json = JsonConvert.SerializeObject(token);
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
#if TEST
            Console.WriteLine("SaveToken End");
#endif
            return token;
        }

        public OAuthToken GetDeviceToken()
        {
#if TEST
            Console.WriteLine("GetDeviceToken Start");
#endif
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = TokenLabel,
                Service = ServiceName
            };

            SecStatusCode resultCode;
            var data = SecKeyChain.QueryAsRecord(existingRecord, out resultCode);

            if (resultCode == SecStatusCode.Success)
            {
#if TEST
            Console.WriteLine("Successfully retrieved record");
#endif
                var json = NSString.FromData(data.ValueData, NSStringEncoding.UTF8);
                var token = JsonConvert.DeserializeObject<OAuthToken>(json);
                CurrentUser.Email = token.Username;
                return token;
            }
#if TEST
            Console.WriteLine("GetDeviceToke End");
#endif
            return null;
        }

        public void DeleteDeviceToken()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = TokenLabel,
                Service = ServiceName
            };

            SecKeyChain.Remove(existingRecord);
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