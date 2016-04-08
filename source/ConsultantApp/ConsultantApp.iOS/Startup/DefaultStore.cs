using System;
using Foundation;
using Shared.Core.Platform;

namespace ConsultantApp.iOS
{
	public class DefaultStore: IDefaultStore
	{
		static string _usernameKey = "Username";

		public string Username { 
			get { 
				return NSUserDefaults.StandardUserDefaults.StringForKey (_usernameKey);
			}
			set { 
				NSUserDefaults.StandardUserDefaults.SetString (value, _usernameKey);
			}
		}
	}
}
	