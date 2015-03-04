using Newtonsoft.Json;
using System.Net;

namespace ClientApp.Core
{
    public class ApiErrorResponse
    {
        HttpStatusCode Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
