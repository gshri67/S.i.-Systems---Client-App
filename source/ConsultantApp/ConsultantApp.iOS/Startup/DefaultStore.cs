using System;
using Foundation;
using Shared.Core.Platform;

namespace ConsultantApp.iOS
{
	public class DefaultStore: IDefaultStore
	{
	    private const string UsernameKey = "Username";
	    private const string IssuedAtKey = "IssuedAt";
	    private const string ExpiresAtKey = "ExpiresAt";
        private const string ExpiresInKey = "ExpiresIn";

		public string Username { 
			get { 
				return NSUserDefaults.StandardUserDefaults.StringForKey (UsernameKey);
			}
			set { 
				NSUserDefaults.StandardUserDefaults.SetString (value, UsernameKey);
			}
		}

	    public string TokenExpiresAt
	    {
            get { return NSUserDefaults.StandardUserDefaults.StringForKey(ExpiresAtKey); }
            set { NSUserDefaults.StandardUserDefaults.SetString(value, ExpiresAtKey); }
	    }

	    public int TokenExpiresIn
	    {
            get { return (int)NSUserDefaults.StandardUserDefaults.IntForKey(ExpiresInKey); }
            set { NSUserDefaults.StandardUserDefaults.SetInt(value, ExpiresInKey); }
	    }

	    public string TokenIssuedAt
	    {
            get { return NSUserDefaults.StandardUserDefaults.StringForKey(IssuedAtKey); }
            set { NSUserDefaults.StandardUserDefaults.SetString(value, IssuedAtKey); }
	    }
	}
}
	