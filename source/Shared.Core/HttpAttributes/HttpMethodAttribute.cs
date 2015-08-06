using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shared.Core.HttpAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        static readonly Regex parameterRegex = new Regex(@"{(.*?)}");

        public string Url { get; set; }

        public HttpMethod Type { get; set; }

        public HttpMethodAttribute(HttpMethod type, string url)
        {
            this.Type = type;
            this.Url = url;
        }

        public string BuildRelativeUrl(object values)
        {
            var url = this.Url;
            if (values == null)
            {
                return url;
            }
            var routeValueDictionary = values.GetType().GetRuntimeProperties()
                .ToDictionary(pi => pi.Name, pi => pi.GetValue(values));
            url = parameterRegex.Replace(url, match =>
            {
                object value;
                var key = match.Groups[1].Value;
                if (routeValueDictionary.TryGetValue(key, out value))
                {
                    routeValueDictionary.Remove(key);
                    return value.ToString();
                }
                return string.Empty;
            });

            var queryString = string.Join("&", routeValueDictionary.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

            return string.Format("{0}{1}{2}", url, string.IsNullOrEmpty(queryString) ? string.Empty : "?", queryString);
        }
    }
}
