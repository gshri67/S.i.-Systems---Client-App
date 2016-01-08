using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        IEnumerable<UserContact> GetClientContacts();
        IEnumerable<int> FindUserIds(string query);
    }

    public class UserContactRepository : IUserContactRepository
    {
        private IEnumerable<EmailAddress> GetUserContactEmailsByUserId(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string emailsQuery =
                    @"SELECT 'Primary' AS Title, PrimaryEmail as Email
                    FROM User_Email
                    WHERE User_Email.UserID = @Id
                    AND PrimaryEmail IS NOT NULL

                    SELECT 'Secondary' AS Title, SecondaryEmail as Email
                    FROM User_Email
                    WHERE User_Email.UserID = @Id
                    AND SecondaryEmail IS NOT NULL";

                var emails = new List<EmailAddress>();

                using (var multi = db.Connection.QueryMultiple(emailsQuery, new { Id = userId }, null))
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var emailsAddress = multi.Read<EmailAddress>().SingleOrDefault();
                        if (emailsAddress != null)
                            emails.Add(emailsAddress);
                    }
                }  

                return emails;
            }
        }

        private IEnumerable<PhoneNumber> GetUserContactPhoneNumbersByUserId(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string multiplePhoneNumbersQuery =
                    @"SELECT 'Home' AS Title, 
	                    CAST(Home_AreaCode AS INT) AS AreaCode, 
	                    CAST(LEFT(Home_Number,3) AS INT) AS Prefix, 
	                    CAST(RIGHT(Home_Number,4) as INT) AS LineNumber
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id
                    AND Home_Number IS NOT NULL

                    SELECT 'Cell' AS Title, 
	                    CAST(Cell_AreaCode AS INT) AS AreaCode, 
	                    CAST(LEFT(Cell_Number,3) AS INT) AS Prefix, 
	                    CAST(RIGHT(Cell_Number,4) as INT) AS LineNumber
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id
                    AND Cell_Number IS NOT NULL

                    SELECT 'Work' AS Title, 
	                    CAST(Work_AreaCode AS INT) AS AreaCode, 
	                    CAST(LEFT(Work_Number,3) AS INT) AS Prefix, 
	                    CAST(RIGHT(Work_Number,4) as INT) AS LineNumber,
	                    CAST(Work_Extension as INT) AS Extension
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id
                    AND Work_Number IS NOT NULL

                    SELECT 'Other' AS Title, 
	                    CAST(Other_AreaCode AS INT) AS AreaCode, 
	                    CAST(LEFT(Other_Number,3) AS INT) AS Prefix, 
	                    CAST(RIGHT(Other_Number,4) as INT) AS LineNumber,
	                    CAST(Other_Extension as INT) AS Extension
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id
                    AND Other_Number IS NOT NULL

                    SELECT 'Fax' AS Title, 
	                    CAST(Fax_AreaCode AS INT) AS AreaCode, 
	                    CAST(LEFT(Fax_Number,3) AS INT) AS Prefix, 
	                    CAST(RIGHT(Fax_Number,4) as INT) AS LineNumber
                    FROM User_Phone
                    WHERE User_Phone.UserID = @Id
                    AND Fax_Number IS NOT NULL";

                var phoneNumbers = new List<PhoneNumber>();

                using (var multi = db.Connection.QueryMultiple(multiplePhoneNumbersQuery, new { Id = userId }, null))
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var phonenumber = multi.Read<PhoneNumber>().SingleOrDefault();
                        if (phonenumber != null)
                            phoneNumbers.Add(phonenumber);
                    }
                }  

                return phoneNumbers.AsEnumerable();
            }
        }

        public UserContact GetUserContactById(int id)
        {
            const string contactByUserIdQuery =
                @"SELECT Users.UserId AS Id,
	                Users.FirstName, 
	                Users.LastName,
	                Company.CompanyName AS ClientName,
	                ISNULL(Addr.Address1, '') 
	                + ISNULL(Addr.Address2, '') 
	                + ISNULL(Addr.Address3, '') 
	                + ISNULL(Addr.Address4, '') AS Address
                FROM Users 
                LEFT JOIN Company ON Users.CompanyID = Company.CompanyID
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
                    @"SELECT Agreement.ContactID  AS Id,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName AS ClientName,
	                    ISNULL(Addr.Address1, '') 
	                    + ISNULL(Addr.Address2, '') 
	                    + ISNULL(Addr.Address3, '') 
	                    + ISNULL(Addr.Address4, '')
                        + ISNULL(Addr.City, '') AS Address
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
                    @"SELECT Agreement.ContactID AS Id,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName AS ClientName,
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

        public IEnumerable<UserContact> GetClientContacts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> FindUserIds(string query)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Users.UserID
                    FROM Users 
                    JOIN PickList ON PickList.PickListID = Users.UserType
                    WHERE ((PickList.PickTypeID IN (SELECT PickTypeID FROM PickType WHERE Type = 'UserRoles') AND PickList.Title='Candidate'))
                    AND (FirstName + ' ' + LastName LIKE  '%'+@Query+'%')
                    UNION
                    SELECT Users.UserID
                    FROM Users
                    JOIN Company ON Company.CompanyID = Users.CompanyID
                    JOIN PickList ON PickList.PickListID = Users.UserType
                    WHERE Company.Inactive = 0
                    AND (PickList.PickTypeID IN (SELECT PickTypeID FROM PickType WHERE Type = 'UserRoles') AND PickList.Title='Client Contact')
                    AND (FirstName + ' ' + LastName LIKE  '%'+@Query+'%')";

                var userIds = db.Connection.Query<int>(contractsQuery, param: new { Query = query });
                
                return userIds ?? Enumerable.Empty<int>();
            }
        }

        public UserContact GetClientContactByAgreementId(int contractId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.ContactID AS Id,
	                    Users.FirstName, 
	                    Users.LastName,
	                    Company.CompanyName AS ClientName,
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
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary"} }.AsEnumerable(),
                PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
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
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
                PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
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
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
                PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
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
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
                PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
                ClientName = "Cenovus",
                Address = "999 Rainbow Road SE, Calgary, AB"
            };
        }

        public IEnumerable<UserContact> GetClientContacts()
        {
            IEnumerable<UserContact> clientContacts = new List<UserContact>
            {
                GetUserContactById(0)
            }.AsEnumerable();

            return clientContacts;
        }

        public IEnumerable<int> FindUserIds(string query)
        {
            throw new NotImplementedException();
        }
    }
}
