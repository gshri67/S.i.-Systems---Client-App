using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class ConsultantContractService
    {
        private IConsultantContractRepository _consultantContractRepository;

        public ConsultantContractService(IConsultantContractRepository consultantContractRepository)
        {
            _consultantContractRepository = consultantContractRepository;
        }

        public IEnumerable<ConsultantContract> GetContracts() 
        {
            IEnumerable<ConsultantContract> repoContracts = _consultantContractRepository.GetContracts();
            List<ConsultantContract> contractList = repoContracts.ToList();

            for (int i = 0; i < contractList.Count; i++)
            {

                if (i < 2)
                    contractList[i].StatusType = MatchGuideConstants.ConsultantContractStatusTypes.Active;
                else if (i < 5)
                    contractList[i].StatusType = MatchGuideConstants.ConsultantContractStatusTypes.Starting;
                else
                    contractList[i].StatusType = MatchGuideConstants.ConsultantContractStatusTypes.Ending;

                if (i % 2 == 0)
                    contractList[i].IsFloThru = true;
                else
                    contractList[i].IsFullySourced = true;

                contractList[i].consultant = new IM_Consultant();
                contractList[i].consultant.FirstName = "Bob";
                contractList[i].consultant.LastName = "Smith";
            }

            return contractList.AsEnumerable<ConsultantContract>();
        }
    }
}
