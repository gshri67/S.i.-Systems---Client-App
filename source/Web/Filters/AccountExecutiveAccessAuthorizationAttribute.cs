using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using SiSystems.ClientApp.Web.Auth;
using System.Net;
using Microsoft.AspNet.Identity.Owin;
using SiSystems.ClientApp.Web;

namespace SiSystems.AccountExecutiveApp.Web.Filters
{
    public class AccountExecutiveAccessAuthorizationAttribute : AuthorizeAttribute
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

            SetActionResponseIfForbidden(actionContext, user);

            return base.IsAuthorized(actionContext);
        }

        private void SetActionResponseIfForbidden(HttpActionContext actionContext, ApplicationUser user)
        {
            if (user == null)
            {
                actionContext.Response = ConstructResponseWithError(actionContext, "User not found.");
            }
            else if (!user.IsInternalUser)
            {
                actionContext.Response = ConstructResponseWithError(actionContext
                    , "Unfortunately, only employees of S.i. Systems have access to this application.");
            }
        }

        private HttpResponseMessage ConstructResponseWithError(HttpActionContext actionContext, string errorMessage)
        {
            return actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new { error = "invalid_access", error_description = errorMessage });
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