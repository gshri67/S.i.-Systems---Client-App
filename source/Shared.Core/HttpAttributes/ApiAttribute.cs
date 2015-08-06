using System;

namespace Shared.Core.HttpAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ApiAttribute : Attribute
    {
        public string BaseUrl { get; set; }

        public ApiAttribute(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }
    }
}
