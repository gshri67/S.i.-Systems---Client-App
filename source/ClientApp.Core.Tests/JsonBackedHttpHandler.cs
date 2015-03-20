using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core.Tests
{
    public class JsonBackedHandler : DelegatingHandler
    {
        readonly Dictionary<string, object> DataSource;

        internal JsonBackedHandler(string filename)
        {
            this.DataSource = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(filename));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var data = DataSource[request.RequestUri.AbsolutePath].ToString();

            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent(data) });
        }
    }
}
