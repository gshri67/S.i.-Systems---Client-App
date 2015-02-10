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
        private readonly ConsultantRepository _repository;
        private readonly ISessionContext _sessionContext;

        public ConsultantService(ConsultantRepository repository, ISessionContext sessionContext)
        {
            _repository = repository;
            _sessionContext = sessionContext;
        }

        public Consultant Find(int id)
        {
            var consultant = _repository.Find(id);

            //filter out any contracts that the current user shouldn't see
            if (consultant != null)
            {
                consultant.Contracts =
                    consultant.Contracts.Where(c => c.ClientId == _sessionContext.CurrentUser.ClientId).ToList();
            }
            
            if (consultant!=null && !consultant.Contracts.Any())
                throw new UnauthorizedAccessException();
            
            return consultant;
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query)
        {
            return _repository.FindAlumni(query, _sessionContext.CurrentUser.ClientId);
        }
    }
}
