﻿using System;
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
    public class ContractCreationSupportService
    {
        private readonly IInternalEmployeesRepository _internalEmployeesRepository;
        private readonly ISessionContext _session;

        public ContractCreationSupportService(ISessionContext session, IInternalEmployeesRepository repo)
        {
            _session = session;
            _internalEmployeesRepository = repo;
        }

        public IEnumerable<InternalEmployee> GetColleaguesForCurrentUser()
        {
            var accountExecutives = _internalEmployeesRepository.GetAccountExecutivesThatShareBranchWithUserId(_session.CurrentUser.Id);
            
            return accountExecutives.OrderBy(employee => employee.FirstName).ThenBy(employee => employee.LastName);
        }

        public ContractCreationOptions GetContractOptionsForJobAndCandidate(int jobId, int candidateId)
        {
            return new ContractCreationOptions
            {
                Colleagues = GetColleaguesForCurrentUser()
            };
        }
    }
}
