using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using Newtonsoft.Json;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class ConnectionService : IConnectionService
    {
        //TODO store and retrieve this from somewhere
        private const string _baseAddr = "http://clientapi.local:50021/";

        //TODO store this somewhere else, save to device
        private static OAuthToken _token;

        public async Task<ValidationResult> Login(string username, string password)
        {
            var request = WebRequest.Create(String.Format("{0}api/Login", _baseAddr));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;

            var data = String.Format("username={0}&password={1}&grant_type=password", WebUtility.HtmlEncode(username), WebUtility.HtmlEncode(password));

            var bytes = Encoding.UTF8.GetBytes(data);
            try
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false,
                    "Unable to connect to server.  Please verify your network connection and try again.");
            }
            

            try
            {
                var httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                string json;
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }

                var token = JsonConvert.DeserializeObject<OAuthJsonToken>(json);
                _token = new OAuthToken
                         {
                             AccessToken = token.AccessToken,
                             ExpiresAt = token.ExpiresAt,
                             ExpiresIn = token.ExpiresIn,
                             IssuedAt = token.IssuedAt,
                             TokenType = token.TokenType,
                             Username =  token.Username
                         };
                return new ValidationResult(true);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "The username or password is incorrect");
            }
        }

        public async Task<string> Get(string service)
        {
            var request = WebRequest.Create(String.Format("{0}api/{1}", _baseAddr, service));
            request.Method = "GET";
            request.ContentType = "application/json";

            if (_token == null)
            {
                //TODO make better
                throw new Exception("No Token");
            }
            request.Headers.Add("Authorization", String.Format("Bearer {0}", _token.AccessToken));


            try
            {
                var httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                string json;
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                return json;
            }
            catch (Exception ex)
            {
                //TODO handle difference between connection interupted and token expired
                //Log that credentials are bad.  Maybe throw exception that forces client back to login screen?
                throw;
            }
        }

        public async Task<string> Post(string service, object data)
        {
            var request = WebRequest.Create(String.Format("{0}api/{1}", _baseAddr, service));
            request.Method = "POST";
            request.ContentType = "application/json";

            if (_token == null)
            {
                //TODO make better
                throw new Exception("No Token");
            }
            request.Headers.Add("Authorization", String.Format("Bearer {0}", _token.AccessToken));

            var dataString = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(dataString);
            try
            {
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            
                var httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                string json;
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                return json;
            }
            catch (Exception ex)
            {
                //TODO handle difference between connection interupted and token expired
                //Log that credentials are bad.  Maybe throw exception that forces client back to login screen?
                throw;
            }
        }

        public OAuthToken Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private class OAuthJsonToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("userName")]
            public string Username { get; set; }

            [JsonProperty(".issued")]
            public string IssuedAt { get; set; }

            [JsonProperty(".expires")]
            public string ExpiresAt { get; set; }
        }
    }
}
