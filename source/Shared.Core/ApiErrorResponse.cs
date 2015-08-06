using System.Net;
using Newtonsoft.Json;

namespace Shared.Core
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
