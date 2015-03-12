using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IClientDetailsRepository
    {
        /// <summary>
        /// ClientAccountDetails containing information regarding the account details of 
        /// the user's client.
        /// </summary>
        /// <param name="clientId">The ID of the client that we are retrieving Client Details for</param>
        ClientAccountDetails GetClientDetails(int clientId);
    }

    public class ClientDetailsRepository : IClientDetailsRepository
    {
        public ClientAccountDetails GetClientDetails(int clientId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var query = @"SELECT FloThruFee"
                              + " ,MSPFeePercentage as MspFeePercentage"
                              + " ,FloThruFeeTypeID as FloThruFeeType"
                              + " ,FloThruFeePaymentID as FloThruFeePayment"
                              + " ,MaxVisibleRatePerHour as MaxVisibleRate"
                              + " ,FloThruMSPPaymentID as FloThruMspPayment"
                          + " FROM [Company]"
                          + " WHERE CompanyID="+clientId.ToString();
                var clientDetails = db.Connection.Query<ClientAccountDetails>(query).FirstOrDefault();
                return clientDetails ?? new ClientAccountDetails();
            }
        }
    }
}
