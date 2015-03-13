using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Auth;

namespace SiSystems.ClientApp.Web.Filters
{
    public class AccessClaimAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = actionContext.Request.GetOwinContext().Authentication.User;
            var response = new HttpResponseMessage();
            var allowedAccessLevels = new MatchGuideConstants.FloThruAlumniAccess[] {
                MatchGuideConstants.FloThruAlumniAccess.AllAccess,
                MatchGuideConstants.FloThruAlumniAccess.LimitedAccess
            };

            var companyClaim = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Company);
            var company = companyClaim != null && !string.IsNullOrEmpty(companyClaim.Value) ? companyClaim.Value : "Client";

            if (!user.HasClaim(claim => claim.Type == CustomClaimTypes.CompanyAccess && claim.Value == bool.TrueString))
            {
                
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    System.Net.HttpStatusCode.Forbidden,
                    new HttpError(
                        new AccessLevelException(string.Format("{0} has not been configured for access to the application.", company)), 
                        true));
            }
            else if (!user.HasClaim(claim => claim.Type == CustomClaimTypes.FloThruAlumniAccessLevel
                        && allowedAccessLevels.Select(c => c.ToString()).Contains(claim.Value)))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    System.Net.HttpStatusCode.Forbidden,
                    new HttpError(new AccessLevelException(string.Format("{0} is configured in the application, but you have not been granted access.", company)), true));
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}
