using SiSystems.ClientApp.SharedModels;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using SiSystems.ClientApp.Web.Auth;
using System.Net;
using Microsoft.AspNet.Identity.Owin;

namespace SiSystems.ClientApp.Web.Filters
{
    public class AccessAuthorizationAttribute : AuthorizeAttribute
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
            if (!user.IsGrantedAccess)
            {
                string errorMessage = user.IsCompanyParticipating
                    ? string.Format("{0} is configured in the application, but you have not been granted access.", user.CompanyName)
                    : string.Format("{0} has not been configured for access to the application.", user.CompanyName);

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new { error = "invalid_access", error_description = errorMessage });
            }

            return base.IsAuthorized(actionContext);
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
