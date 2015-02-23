using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain
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

        public ConsultantService(IConsultantRepository consultantRepository, ICompanyRepository companyRepository, ISessionContext sessionContext)
        {
            _consultantRepository = consultantRepository;
            _companyRepository = companyRepository;
            _sessionContext = sessionContext;
        }

        public Consultant Find(int id)
        {
            var consultant = _consultantRepository.Find(id);

            var associatedCompanyIds =
                _companyRepository.GetAllAssociatedCompanyIds(_sessionContext.CurrentUser.ClientId);

            //filter out any contracts that the current user shouldn't see
            if (consultant != null)
            {
                consultant.Contracts =
                    consultant.Contracts.Where(c => associatedCompanyIds.Contains(c.ClientId)).ToList();
            }

            if (consultant != null && !consultant.Contracts.Any())
                throw new UnauthorizedAccessException();

            return consultant;
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query)
        {
            var associatedCompanyIds =
                _companyRepository.GetAllAssociatedCompanyIds(_sessionContext.CurrentUser.ClientId);

            var results = _consultantRepository.FindAlumni(query, associatedCompanyIds);
            var orderedResults = OrderAlumniGroups(results);

            return orderedResults;
        }

        private IOrderedEnumerable<ConsultantGroup> OrderAlumniGroups(IEnumerable<ConsultantGroup> results)
        {
            //order results by group length
            var orderedResults = results.OrderByDescending(g => g.Consultants.Count);

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
            switch (rating.GetValueOrDefault(0))
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
