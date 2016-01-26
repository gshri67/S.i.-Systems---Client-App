using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class AccountExecutiveService
    {
        private readonly IInternalEmployeesRepository _repo;
        private readonly ISessionContext _session;

        public AccountExecutiveService(ISessionContext session, IInternalEmployeesRepository repo)
        {
            _session = session;
            _repo = repo;
        }

        public IEnumerable<InternalEmployee> GetAccountExecutives()
        {
            var accountExecutives = _repo.GetAccountExecutivesThatShareBranchWithUserId(_session.CurrentUser.Id);
            
            return accountExecutives.OrderBy(employee => employee.FirstName).ThenBy(employee => employee.LastName);
        }
    }

    public class InternalEmployee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
    }
}
