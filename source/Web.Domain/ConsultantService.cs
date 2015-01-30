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
            if (consultant!=null && consultant.ClientId != _sessionContext.CurrentUser.ClientId)
                throw new UnauthorizedAccessException();
            
            return consultant;
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query)
        {
            var result = new List<ConsultantGroup>();
            var groupedResults = _repository.FindAlumni(query, _sessionContext.CurrentUser.ClientId);
            foreach (var group in groupedResults)
            {
                result.Add(new ConsultantGroup
                {
                    GroupTitle = group.Key,
                    Contractors = group.ToList()
                });
            }
            return result;
        }
    }
}
