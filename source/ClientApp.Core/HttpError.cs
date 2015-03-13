using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    /// <summary>
    /// A simplified PCL compatible object with the same properties 
    /// as System.Web.Http.HttpError to use for deserialization on the client
    /// </summary>
    public class HttpError
    {
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; }
        public string StackTrace { get; set; }
    }
}
