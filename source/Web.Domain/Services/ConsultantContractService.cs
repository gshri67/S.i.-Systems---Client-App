using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public ConsultantContractService()
        {
        }

        public IEnumerable<ConsultantContract> GetContracts() 
        {
            List<ConsultantContract> contractList = new List<ConsultantContract>();

            for (int i = 0; i < 19; i++)
            {
                ConsultantContract contract = new ConsultantContract();
                contract.consultant = new IM_Consultant();

                contract.consultant.FirstName = "Bob";
                contract.consultant.LastName = "Smith";

                contract.EndDate = new DateTime(2015, 12, 26);

                if (i < 7)
                    contract.StatusType = MatchGuideConstants.ContractStatusTypes.Active;
                else if( i < 14 )
                    contract.StatusType = MatchGuideConstants.ContractStatusTypes.Pending;
                else
                    contract.StatusType = MatchGuideConstants.ContractStatusTypes.Expired;

                if (i % 2 == 0)
                    contract.IsFloThru = true;
                else
                    contract.IsFullySourced = true;

                //job.isProposed = (i % 3) == 0;
                //job.hasCallout = job.isProposed && ((i%2) == 0);
                contract.Title = "Developer" + i.ToString();
             
                contractList.Add(contract);
            }
            return contractList.AsEnumerable<ConsultantContract>();
        }
    }
}
