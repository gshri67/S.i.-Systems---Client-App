using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SiSystems.ClientApp.Web.Filters
{
    public class AccessAuthorizationAttribute : AuthorizeAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var sessionContext = (ISessionContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISessionContext));
            var user = sessionContext.CurrentUser;
      
            if (!sessionContext.IsAuthorized)
            {
                string errorMessage = sessionContext.CurrentUserDetails.HasAccess
                    ? string.Format("{0} is configured in the application, but you have not been granted access.", user.CompanyName)
                    : string.Format("{0} has not been configured for access to the application.", user.CompanyName);

                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    System.Net.HttpStatusCode.Forbidden,
                    new HttpError(new AccessLevelException(errorMessage), true));
            }

            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
    }
}
