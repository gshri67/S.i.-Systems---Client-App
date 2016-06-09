using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using SiSystems.ClientApp.Web;

namespace SiSystems.ConsultantApp.Web.Filters
{
    public class ConsultantAccessAuthorizationAttribute : AuthorizeAttribute
    {

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var userName = GetUserName(actionContext.RequestContext);
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            var userManager = actionContext.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByNameAsync(userName).Result;

            if (user == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new { error = "invalid_access", error_description = "User not found." });
            }
            var baseAuthorized = base.IsAuthorized(actionContext);
            if(!baseAuthorized)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { error = "invalid_access", error_description = "Something in the context says no." });
            return baseAuthorized;
        }

        private string GetUserName(HttpRequestContext context)
        {
            if (context.Principal != null && context.Principal.Identity != null)
            {
                return context.Principal.Identity.Name;
            }
            return null;
        }
    }
}