using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.ConsultantApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Controllers.Api
{
    [ConsultantAccessAuthorizationAttribute]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController: ApiController
    {
        public  DashboardController()
        {

        }

        public HttpResponseMessage getDashboardInfo()
        {
            //var info = _service.getDashboardInfo();
            var info = new DashboardInfo();

            info.FS_curContracts = 55;
            info.FS_startingContracts = 17;
            info.FS_endingContracts = 12;

            info.FT_curContracts = 55;
            info.FT_startingContracts = 17;
            info.FT_endingContracts = 12;

            info.curJobs = 18;
            info.calloutJobs = 6;
            info.proposedJobs = 9;

            return Request.CreateResponse(HttpStatusCode.OK, info);
        }
    }
}