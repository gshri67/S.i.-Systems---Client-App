using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Newtonsoft.Json;
using Security;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS.Startup
{
    public static class TokenStore
    {
        public static void SaveToken(OAuthToken token)
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
        }

        public static OAuthToken GetDeviceToken()
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
                return token;
            }
            return null;
        }

        public static void DeleteDeviceToken()
        {
            var existingRecord = new SecRecord(SecKind.GenericPassword)
            {
                Label = "Certificate",
                Service = "SiSystemsClientApp"
            };

            SecKeyChain.Remove(existingRecord);
        }
    }
}