using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IUserContactRepository
    {
        UserContact GetUserContactById(int id);
        UserContact GetDirectReportByAgreementId(int contractId);
        UserContact GetClientContactByAgreementId(int contractId);
        UserContact GetBillingContactByAgreementId(int contractId);
    }

    public class UserContactRepository : IUserContactRepository
    {
        public UserContact GetUserContactById(int id)
        {
            throw new NotImplementedException();
        }

        public UserContact GetDirectReportByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT
	                    directReport.FirstName FirstName,
                        directReport.LastName LastName,
                        directReportEmail.PrimaryEmail EmailAddress
                    FROM [Agreement] agr
                    JOIN [Agreement_ContractAdminContactMatrix] agrContact on agrContact.AgreementID = agr.AgreementID
	                LEFT JOIN [Users] directReport ON agrContact.DirectReportUserID = directReport.UserID
                    JOIN [User_Email] directReportEmail on directReportEmail.UserID = directReport.UserID
                    WHERE agr.AgreementID = @Id";

                var contact = db.Connection.Query<UserContact>(contractsQuery, param: new {Id = contractId}).FirstOrDefault();

                return contact;
            }
        }

        public UserContact GetBillingContactByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT
	                    billingContact.FirstName FirstName,
                        billingContact.LastName LastName,
                        billingContactEmail.PrimaryEmail EmailAddress
                    FROM [Agreement] agr
                    JOIN [Agreement_ContractAdminContactMatrix] agrContact on agrContact.AgreementID = agr.AgreementID
	                LEFT JOIN [Users] billingContact ON agrContact.BillingUserID = billingContact.UserID
                    JOIN [User_Email] billingContactEmail on billingContactEmail.UserID = billingContact.UserID
                    WHERE agr.AgreementID = @Id";

                var contacts = db.Connection.Query<UserContact>(contractsQuery, param: new { Id = contractId });

                return contacts.FirstOrDefault();
            }
        }

        public UserContact GetClientContactByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT
	                    clientContact.FirstName FirstName,
                        clientContact.LastName LastName,
                        clientContactEmail.PrimaryEmail EmailAddress
                    FROM [Agreement] agr
                    LEFT JOIN [Users] clientContact ON agr.CandidateID = clientContact.UserID
                    JOIN [User_Email] clientContactEmail on clientContactEmail.UserID = clientContact.UserID
                    WHERE agr.AgreementID = @Id";

                var contacts = db.Connection.Query<UserContact>(contractsQuery, param: new { Id = contractId });

                return contacts.FirstOrDefault();
            }
        }
    }

    public class MockUserContactRepository : IUserContactRepository
    {
        public UserContact GetUserContactById(int id)
        {
            return new UserContact
            {
                Id = id,
                FirstName = "Robert",
                LastName = "Paulson",
                EmailAddresses = new List<string>() { "rp.clientcontact@email.com" }.AsEnumerable(),
                PhoneNumbers = new List<string>() { "(555)555-1231", "(555)222-2212" }.AsEnumerable(),
                ClientName = "Cenovus",
                Address = "999 Rainbow Road SE, Calgary, AB",
                ContactType = UserContactType.ClientContact
            };
        }

        public UserContact GetDirectReportByAgreementId(int contractId)
        {
            return new UserContact
            {
                Id = contractId,
                FirstName = "Robert",
                LastName = "Paulson",
                EmailAddresses = new List<string>() { "rp.clientcontact@email.com" }.AsEnumerable(),
                PhoneNumbers = new List<string>() { "(555)555-1231", "(555)222-2212" }.AsEnumerable(),
                ClientName = "Cenovus",
                Address = "999 Rainbow Road SE, Calgary, AB",
                ContactType = UserContactType.ClientContact
            };
        }

        public UserContact GetClientContactByAgreementId(int contractId)
        {
            return new UserContact
            {
                Id = contractId,
                FirstName = "Robert",
                LastName = "Paulson",
                EmailAddresses = new List<string>() { "rp.clientcontact@email.com" }.AsEnumerable(),
                PhoneNumbers = new List<string>() { "(555)555-1231", "(555)222-2212" }.AsEnumerable(),
                ClientName = "Cenovus",
                Address = "999 Rainbow Road SE, Calgary, AB",
                ContactType = UserContactType.ClientContact
            };
        }

        public UserContact GetBillingContactByAgreementId(int contractId)
        {
            return new UserContact
            {
                Id = contractId,
                FirstName = "Robert",
                LastName = "Paulson",
                EmailAddresses = new List<string>() { "rp.clientcontact@email.com" }.AsEnumerable(),
                PhoneNumbers = new List<string>() { "(555)555-1231", "(555)222-2212" }.AsEnumerable(),
                ClientName = "Cenovus",
                Address = "999 Rainbow Road SE, Calgary, AB",
                ContactType = UserContactType.ClientContact
            };
        }
    }
}
