using System;
using Foundation;
using Security;
using Shared.Core;
using Shared.Core.Platform;

namespace ConsultantApp.iOS.Startup
{
    public class TokenStore : ITokenStore
    {
        private const string ServiceName = "com.sisystems.ConsultantApp";
        private const string TokenLabel = "AuthToken";

		private SecRecord query = new SecRecord (SecKind.GenericPassword) { Label = TokenLabel };

        public bool SaveToken(string username, string token)
		{
            var newRecord = new SecRecord(SecKind.GenericPassword)
            {
                Service = ServiceName,
                Label = TokenLabel,
                Account = username,
                ValueData = NSData.FromString(token),
				Accessible = SecAccessible.WhenUnlocked
            };

			//Check to see if there is an item already
			//If there is, we'll update; otherwise we'll add
			if (GetDeviceToken () != null) {
				return SecKeyChain.Update (query, newRecord) == SecStatusCode.Success;
			} else {
				return SecKeyChain.Add (newRecord) == SecStatusCode.Success;
			}
        }

        public string GetDeviceToken()
        {
			SecStatusCode resultCode;
			var data = SecKeyChain.QueryAsRecord(query, out resultCode);

			if (resultCode != SecStatusCode.Success)
				return null;

			return NSString.FromData(data.ValueData, NSStringEncoding.UTF8);
        }

        public void DeleteDeviceToken()
        {
			SecKeyChain.Remove(query);
		}
	
    }
}