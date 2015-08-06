using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core.HttpAttributes
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
