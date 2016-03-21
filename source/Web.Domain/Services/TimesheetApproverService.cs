using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class TimesheetApproverService
    {
        private readonly ISessionContext _sessionContext;
        private readonly IDirectReportRepository _timeSheetApproverRepository;
        private readonly ICompanyRepository _companyRepository;

        public TimesheetApproverService(ISessionContext sessionContext , IDirectReportRepository directReportRepository, ICompanyRepository companyRepository)
        {
            _sessionContext = sessionContext;
            _timeSheetApproverRepository = directReportRepository;
            _companyRepository = companyRepository;
        }

        public IEnumerable<DirectReport> GetTimesheetApproversByAgreementId( int agreementId )
        {
            var companyId = _companyRepository.GetCompanyIdByAgreementId(agreementId);
            var allAssociatedCompanyIds = this._companyRepository.GetAllAssociatedCompanyIds(companyId);

            var timesheetApprovers = new List<DirectReport>();
            
            foreach (var id in allAssociatedCompanyIds)
            {
                timesheetApprovers.AddRange(_timeSheetApproverRepository.GetTimesheetApproversByCompanyId(id));
            }

            return timesheetApprovers.GroupBy(report => report.Id).Select(group=>group.FirstOrDefault());
        }
    }
}
