using System.Linq;
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
        private readonly ICompanyRepository _companyRepository;

        public ClientDetailsRepository(ICompanyRepository companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        public ClientAccountDetails GetClientDetails(int clientId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var query = @"SELECT CompanyId as ClientId," 
                              + "  FloThruFee"
                              + " ,MSPFeePercentage as MspFeePercentage"
                              + " ,FloThruFeeTypeID as FloThruFeeType"
                              + " ,FloThruFeePaymentID as FloThruFeePayment"
                              + " ,MaxVisibleRatePerHour as MaxVisibleRate"
                              + " ,FloThruMSPPaymentID as FloThruMspPayment"
                              + " ,CompanyInvoiceFrequencyID as ClientInvoiceFrequency"
                              + " ,IsHavingFTAlumni as HasAccess"
                          + " FROM [Company]"
                          + " WHERE CompanyID="+clientId.ToString();
                var details = db.Connection.Query<ClientAccountDetails>(query).FirstOrDefault() 
                    ?? new ClientAccountDetails();

                // Temporary work around to get list of participating 
                // companies until the matchguide database is updated
                if (Settings.ShouldUseConfiguredParticipatingCompaniesList)
                {
                    var associatedCompanies = this._companyRepository.GetAllAssociatedCompanyIds(details.ClientId);
                    details.HasAccess = Settings.ParticipatingCompaniesList.Values.Any(v => associatedCompanies.Contains(v));
                }

                return details;
            }
        }
    }
}
