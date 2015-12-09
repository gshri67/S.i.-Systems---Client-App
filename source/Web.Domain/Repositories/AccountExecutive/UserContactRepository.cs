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
        private IEnumerable<string> GetUserContactEmailsByUserId(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string emailsQuery =
                    @"SELECT PrimaryEmail
                    FROM User_Email
                    WHERE User_Email.UserID = @Id";

                var emails = db.Connection.Query<string>(emailsQuery, param: new { Id = userId });

                return emails;
            }
        }

        private IEnumerable<string> GetUserContactPhoneNumbersByUserId(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string homePhoneNumbersQuery =
                    @"SELECT '('+cast(user_phone.home_areacode as varchar)+ ')'+ 
	                    cast(left(user_phone.home_number,3) as varchar) + '-' + 
	                    cast(right(user_phone.home_number,4) as varchar)
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id";

                const string cellPhoneNumbersQuery =
                    @"SELECT '('+cast(user_phone.cell_areacode as varchar)+ ')'+ 
	                    cast(left(user_phone.cell_number,3) as varchar) + '-' + 
	                    cast(right(user_phone.cell_number,4) as varchar)
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id";

                const string workPhoneNumbersQuery =
                    @"SELECT '('+cast(user_phone.work_areacode as varchar)+ ')'+ 
	                    cast(left(user_phone.work_number,3) as varchar) + '-' + 
	                    cast(right(user_phone.work_number,4) as varchar)
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id";

                var phoneNumbers = new List<string>();

                var homePhoneNumber = db.Connection.Query<string>(homePhoneNumbersQuery, param: new { Id = userId }).FirstOrDefault();
                if (homePhoneNumber!= null)
                    phoneNumbers.Add(homePhoneNumber);

                var workPhoneNumber = db.Connection.Query<string>(workPhoneNumbersQuery, param: new { Id = userId }).FirstOrDefault();
                if(workPhoneNumber != null)
                    phoneNumbers.Add(workPhoneNumber);

                var cellPhoneNumber = db.Connection.Query<string>(cellPhoneNumbersQuery, param: new { Id = userId }).FirstOrDefault();
                if(cellPhoneNumber != null)
                    phoneNumbers.Add(cellPhoneNumber);

                return phoneNumbers.AsEnumerable();
            }
        }

        public UserContact GetUserContactById(int id)
        {
            const string contactByUserIdQuery =
                @"SELECT Users.UserId AS ContactId,
	                Users.FirstName, 
	                Users.LastName,
	                Company.CompanyName,
	                ISNULL(Addr.Address1, '') 
	                + ISNULL(Addr.Address2, '') 
	                + ISNULL(Addr.Address3, '') 
	                + ISNULL(Addr.Address4, '') AS Address
                FROM Users 
                JOIN Company ON Users.CompanyID = Company.CompanyID
                LEFT JOIN (
	                SELECT *
	                FROM User_Address
	                WHERE User_Address.MainAddress = 1 
	                AND User_Address.Inactive = 0
                ) AddressMatrix ON Users.UserID = AddressMatrix.AddressID
                LEFT JOIN (
	                SELECT *
	                FROM Address	
	                WHERE Address.Inactive = 0
                ) Addr ON AddressMatrix.AddressID = Addr.AddressID
                WHERE Users.UserID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var contact = db.Connection.Query<UserContact>(contactByUserIdQuery, param: new { Id = id }).FirstOrDefault();

                if (contact != null)
                {
                    contact.EmailAddresses = GetUserContactEmailsByUserId(contact.Id);
                    contact.PhoneNumbers = GetUserContactPhoneNumbersByUserId(contact.Id);
                }

                return contact;
            }
        }

        public UserContact GetDirectReportByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.ContactID,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName,
	                    ISNULL(Addr.Address1, '') 
	                    + ISNULL(Addr.Address2, '') 
	                    + ISNULL(Addr.Address3, '') 
	                    + ISNULL(Addr.Address4, '') AS Address
                    FROM Agreement
                    JOIN Agreement_ContractAdminContactMatrix Matrix on Matrix.AgreementID = Agreement.AgreementID
                    JOIN Users ON Matrix.DirectReportUserID = Users.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    LEFT JOIN (
	                    SELECT *
	                    FROM User_Address
	                    WHERE User_Address.MainAddress = 1 
	                    AND User_Address.Inactive = 0
                    ) AddressMatrix ON Users.UserID = AddressMatrix.AddressID
                    LEFT JOIN (
	                    SELECT * 
	                    FROM Address	
	                    WHERE Address.Inactive = 0
                    ) Addr ON AddressMatrix.AddressID = Addr.AddressID
                    WHERE Agreement.AgreementID = @Id";
                
                var contact = db.Connection.Query<UserContact>(contractsQuery, param: new {Id = contractId}).FirstOrDefault();
                
                if (contact != null)
                {
                    contact.EmailAddresses = GetUserContactEmailsByUserId(contact.Id);
                    contact.PhoneNumbers = GetUserContactPhoneNumbersByUserId(contact.Id);
                }

                return contact;
            }
        }

        public UserContact GetBillingContactByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.ContactID,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName,
	                    ISNULL(Addr.Address1, '') 
	                    + ISNULL(Addr.Address2, '') 
	                    + ISNULL(Addr.Address3, '') 
	                    + ISNULL(Addr.Address4, '') AS Address
                    FROM Agreement
                    JOIN Agreement_ContractAdminContactMatrix Matrix on Matrix.AgreementID = Agreement.AgreementID
                    JOIN Users ON Matrix.BillingUserID = Users.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    LEFT JOIN (
	                    SELECT *
	                    FROM User_Address
	                    WHERE User_Address.MainAddress = 1 
	                    AND User_Address.Inactive = 0
                    ) AddressMatrix ON Users.UserID = AddressMatrix.AddressID
                    LEFT JOIN (
	                    SELECT *
	                    FROM Address	
	                    WHERE Address.Inactive = 0
                    ) Addr ON AddressMatrix.AddressID = Addr.AddressID
                    WHERE Agreement.AgreementID = @Id";

                var contact = db.Connection.Query<UserContact>(contractsQuery, param: new { Id = contractId }).FirstOrDefault();

                if (contact != null)
                {
                    contact.EmailAddresses = GetUserContactEmailsByUserId(contact.Id);
                    contact.PhoneNumbers = GetUserContactPhoneNumbersByUserId(contact.Id);
                }

                return contact;
            }
        }

        public UserContact GetClientContactByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.ContactID,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName,
	                    ISNULL(Addr.Address1, '') 
	                    + ISNULL(Addr.Address2, '') 
	                    + ISNULL(Addr.Address3, '') 
	                    + ISNULL(Addr.Address4, '') AS Address
                    FROM Agreement
                    JOIN Users ON Agreement.ContactID = Users.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    LEFT JOIN (
	                    SELECT *
	                    FROM User_Address
	                    WHERE User_Address.MainAddress = 1 
	                    AND User_Address.Inactive = 0
                    ) AddressMatrix ON Users.UserID = AddressMatrix.AddressID
                    LEFT JOIN (
	                    SELECT *
	                    FROM Address	
	                    WHERE Address.Inactive = 0
                    ) Addr ON AddressMatrix.AddressID = Addr.AddressID
                    WHERE Agreement.AgreementID = @Id";

                var contact = db.Connection.Query<UserContact>(contractsQuery, param: new { Id = contractId }).FirstOrDefault();

                if (contact != null)
                {
                    contact.EmailAddresses = GetUserContactEmailsByUserId(contact.Id);
                    contact.PhoneNumbers = GetUserContactPhoneNumbersByUserId(contact.Id);
                }

                return contact;
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
                Address = "999 Rainbow Road SE, Calgary, AB"
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
                Address = "999 Rainbow Road SE, Calgary, AB"
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
                Address = "999 Rainbow Road SE, Calgary, AB"
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
                Address = "999 Rainbow Road SE, Calgary, AB"
            };
        }
    }
}
