using System.Collections.Generic;
using System.Web.Http;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    [Authorize]
    public class ValuesController: ApiController
    {
        public IEnumerable<dynamic> Get()
        {
            return new List<dynamic>
            {
                new {a="asd"},
                new {a="dsa"}
            };
        } 
    }
}
