using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class ConsultantService
    {
        private readonly IConsultantRepository _consultantRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISessionContext _sessionContext;

        public ConsultantService(IConsultantRepository consultantRepository, 
            ICompanyRepository companyRepository, ISessionContext sessionContext)
        {
            _consultantRepository = consultantRepository;
            _companyRepository = companyRepository;
            _sessionContext = sessionContext;
        }

        public Consultant Find(int id)
        {
            var consultant = _consultantRepository.Find(id);

            AssertCurrentUserCanAccessConsultantRecord(consultant);

            TrimConsultantRatesBasedOnMaxVisibleRate(consultant);

            return consultant;
        }
        
        /// <summary>
        /// Validate that current user works for or is associated with a company
        /// that the consultant has worked for
        /// </summary>
        private void AssertCurrentUserCanAccessConsultantRecord(Consultant consultant)
        {
            var associatedCompanyIds =
                _companyRepository.GetAllAssociatedCompanyIds(_sessionContext.CurrentUser.CompanyId);

            if (consultant != null)
            {
                consultant.Contracts =
                    consultant.Contracts.Where(c => associatedCompanyIds.Contains(c.ClientId)).ToList();
            }

            if (consultant != null && !consultant.Contracts.Any())
                throw new UnauthorizedAccessException();

        }

        private IEnumerable<ConsultantGroup> Find(string query, bool active)
        {
            var associatedCompanyIds =
                _companyRepository.GetAllAssociatedCompanyIds(_sessionContext.CurrentUser.CompanyId);

            var results = _consultantRepository.Find(query, associatedCompanyIds, active);
            var orderedResults = OrderAlumniGroups(results);

            TrimRatesBasedOnMaxVisibleRate(orderedResults);

            return orderedResults;
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query)
        {
            return this.Find(query, false);
        }

        public IEnumerable<ConsultantGroup> FindActive(string query)
        {
            return this.Find(query, true);
        }

        private bool RateExceedsMaxVisibleRate(decimal rate)
        {
            return rate > _sessionContext.CurrentUser.ClientsMaxVisibleRate;
        }

        private void TrimRatesBasedOnMaxVisibleRate(IOrderedEnumerable<ConsultantGroup> orderedResults)
        {
            if (!ShouldBeTrimmed())
                return;
            
            foreach (var consultant in from consultantGroup 
                    in orderedResults from consultant in consultantGroup.Consultants
                    where RateExceedsMaxVisibleRate(consultant.MostRecentContractRate) 
                    select consultant)
            {
                consultant.RateWitheld = true;
                consultant.MostRecentContractRate = decimal.Zero;
            }
        }

        private void TrimConsultantRatesBasedOnMaxVisibleRate(Consultant consultant)
        {
            if (!ShouldBeTrimmed())
                return;
            foreach (var contract in consultant.Contracts.Where(contract => RateExceedsMaxVisibleRate(contract.Rate) ))
            {
                contract.Rate = decimal.Zero;
                contract.RateWitheld = true;
            }
        }

        private bool ShouldBeTrimmed()
        {
            return _sessionContext.CurrentUser.FloThruAlumniAccess != MatchGuideConstants.FloThruAlumniAccess.AllAccess && _sessionContext.CurrentUser.ClientsMaxVisibleRate.HasValue;
        }

        private IOrderedEnumerable<ConsultantGroup> OrderAlumniGroups(IEnumerable<ConsultantGroup> results)
        {
            //order results by group length
            var orderedResults = results.OrderBy(g => string.IsNullOrEmpty(g.Specialization)).ThenByDescending(g => g.Consultants.Count);

            foreach (var group in orderedResults)
            {
                group.Consultants = group.Consultants
                    .OrderByDescending(c => ConvertResumeRatingToSortableInt(c.Rating))
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName).ToList();
            }
            return orderedResults;
        }

        private int ConvertResumeRatingToSortableInt(MatchGuideConstants.ResumeRating? rating)
        {
            if (!rating.HasValue) return -1;
            switch (rating.Value)
            {
                case MatchGuideConstants.ResumeRating.AboveStandard:
                    return 3;
                case MatchGuideConstants.ResumeRating.Standard:
                    return 2;
                case MatchGuideConstants.ResumeRating.BelowStandard:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
