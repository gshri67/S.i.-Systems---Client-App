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
                var query = @"SELECT InvoiceFormatId as InvoiceFormat"
                              //+ " ,InvoiceFrequency"
                              + " ,CompanyInvoiceFrequencyID as InvoiceFrequency"
                              + " ,FloThruFeeTypeID as FloThruFeeType"
                              + " ,FloThruFee"
                              + " ,FloThruFeePaymentID as FloThruFeePayment"
                              + " ,MSPFeePercentage as MspFeePercentage"
                              + " ,MaxVisibleRatePerHour as MaxVisibleRate"
                          + " FROM [Company]"
                          + " WHERE CompanyID="+clientId.ToString();
                var clientDetails = db.Connection.Query<ClientAccountDetails>(query).FirstOrDefault();
                return clientDetails ?? new ClientAccountDetails();
            }
        }
    }
}
