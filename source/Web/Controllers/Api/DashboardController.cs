using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Services;
using SiSystems.AccountExecutiveApp.Web.Filters;
using SiSystems.SharedModels;

namespace SiSystems.AccountExecutiveApp.Web.Controllers.Api
{
    [AccountExecutiveAccessAuthorization]
    [RoutePrefix("api/Dashboard")]
    public class DashboardController: ApiController
    {
        private readonly DashboardService _service;
        public  DashboardController(DashboardService service)
        {
            _service = service;
        }

        public HttpResponseMessage GetDashboardInfo()
        {
            var info = _service.GetDashboardSummary();
            return Request.CreateResponse(HttpStatusCode.OK, info);
        }
    }
}