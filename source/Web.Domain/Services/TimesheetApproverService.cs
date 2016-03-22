using System.Collections.Generic;
using System.Configuration;
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
            var timesheetApprovers = GetPossibleDirectReportForAgreementId(agreementId);

            timesheetApprovers = RemoveDuplicateUsers(timesheetApprovers);

            SetFrequentlyUsed(agreementId, timesheetApprovers);

            return timesheetApprovers.OrderBy(report => report.IsFrequentlyUsed).ThenBy(report => report.Email);
        }

        private IEnumerable<DirectReport> GetPossibleDirectReportForAgreementId(int agreementId)
        {
            var timesheetApprovers = new List<DirectReport>();
            var companyId = _companyRepository.GetCompanyIdByAgreementId(agreementId);
            var allAssociatedCompanyIds = _companyRepository.GetAllAssociatedCompanyIds(companyId);

            foreach (var id in allAssociatedCompanyIds)
            {
                timesheetApprovers.AddRange(_timeSheetApproverRepository.GetTimesheetApproversByCompanyId(id));
            }
            return timesheetApprovers;
        }

        private static IEnumerable<DirectReport> RemoveDuplicateUsers(IEnumerable<DirectReport> timesheetApprovers)
        {
            timesheetApprovers =
                timesheetApprovers.GroupBy(report => report.Id).Select(group => @group.FirstOrDefault()).ToList();
            return timesheetApprovers;
        }

        private void SetFrequentlyUsed(int agreementId, IEnumerable<DirectReport> timesheetApprovers)
        {
            var frequentApproverIds = _timeSheetApproverRepository.FrequentyDirectReportIdsByAgreementId(agreementId).ToList();
            foreach (var approver in timesheetApprovers)
            {
                approver.IsFrequentlyUsed = frequentApproverIds.Contains(approver.Id);
            }
        }
    }
}
