using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IInternalEmployeesRepository
    {
        IEnumerable<InternalEmployee> GetAccountExecutivesThatShareBranchWithUserId(int id);
        IEnumerable<InternalEmployee> GetClientContactsWithCompanyId(int companyId); 
        IEnumerable<InternalEmployee> GetBillingContactsWithCompanyId(int companyId); 
        IEnumerable<InternalEmployee> GetDirectReportsWithCompanyId(int companyId);
        IEnumerable<InternalEmployee> GetInvoiceRecipientsWithAgreementId(int agreementId);
    }

    public class InternalEmployeesRepository : IInternalEmployeesRepository
    {
        public IEnumerable<InternalEmployee> GetAccountExecutivesThatShareBranchWithUserId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractSummaryQuery =
                    @"SELECT UserId as Id, FirstName, LastName, Title 
                    FROM Users 
                    WHERE Users.UserOfficeID IN 
                    (
	                    SELECT UserOfficeID FROM Users WHERE UserID = @Id
                    )
                    AND Title IN ('Account Executive','Managing Director')";

                var employees = db.Connection.Query<InternalEmployee>(contractSummaryQuery, new { Id = id });

                return employees;
            }
        }


        public IEnumerable<InternalEmployee> GetClientContactsWithCompanyId(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"
                    select
                    users.userid as Id,
                    users.firstname as FirstName,
                    users.lastname as LastName

                    from users 
                    inner join 
                    company on 
                    company.companyid = users.companyid
                    left join 
                    user_email on 
                    user_email.userid = users.userid
                    left join 
                    picklist on 
                    picklist.picklistid = users.ClientPortalTypeID

                    where
                    company.companyid in
                    (
                        select
                    companyid
                    from[udf_Getalldivisionsforcompany](@companyid)
                    )
                    and
                    users.usertype =
                        (
                            select
                    picklist.picklistid
                        from 
                    picklist
                        where 
                    picklist.picktypeid =
                        (
                            select
                    picktypeid
                        from 
                    picktype
                    where type = 'UserRoles'
                    )
                    and
                    picklist.inactive = 0
                    and title = 'Client Contact'
                    )
                    and
                    users.inactive = 0

                    order by 
                    users.firstname,
                    users.lastname";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyId = companyId });

                return result;
            }
        }

        public IEnumerable<InternalEmployee> GetBillingContactsWithCompanyId(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"
                    select
                    users.userid as Id,
                    users.firstname as FirstName,
                    users.lastname as LastName

                    from users 
                    inner join 
                    company on 
                    company.companyid = users.companyid
                    left join 
                    user_email on 
                    user_email.userid = users.userid
                    left join 
                    picklist on 
                    picklist.picklistid = users.ClientPortalTypeID

                    where
                    company.companyid in
                    (
                        select
                    companyid
                    from[udf_Getalldivisionsforcompany](@companyid)
                    )
                    and
                    users.usertype =
                        (
                            select
                    picklist.picklistid
                        from 
                    picklist
                        where 
                    picklist.picktypeid =
                        (
                            select
                    picktypeid
                        from 
                    picktype
                    where type = 'UserRoles'
                    )
                    and
                    picklist.inactive = 0
                    and title = 'Billing Contact'
                    )
                    and
                    users.inactive = 0

                    order by 
                    users.firstname,
                    users.lastname";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyId = companyId });

                return result;
            }
        }

        public IEnumerable<InternalEmployee> GetDirectReportsWithCompanyId(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"
                    select
                    users.userid as Id,
                    users.firstname as FirstName,
                    users.lastname as LastName

                    from users 
                    inner join 
                    company on 
                    company.companyid = users.companyid
                    left join 
                    user_email on 
                    user_email.userid = users.userid
                    left join 
                    picklist on 
                    picklist.picklistid = users.ClientPortalTypeID

                    where
                    company.companyid in
                    (
                        select
                    companyid
                    from[udf_Getalldivisionsforcompany](@companyid)
                    )
                    and
                    users.usertype =
                        (
                            select
                    picklist.picklistid
                        from 
                    picklist
                        where 
                    picklist.picktypeid =
                        (
                            select
                    picktypeid
                        from 
                    picktype
                    where type = 'UserRoles'
                    )
                    and
                    picklist.inactive = 0
                    and title = 'Direct Report'
                    )
                    and
                    users.inactive = 0

                    order by 
                    users.firstname,
                    users.lastname";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyId = companyId });

                return result;
            }
        }

        public IEnumerable<InternalEmployee> GetInvoiceRecipientsWithAgreementId(int agreementId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"
                        select InvoiceRecipientID,ClientContactID	
                        from agreement_invoicerecipients
                        where Agreement_InvoiceRecipients.AgreementID = @agrId
                        AND Agreement_InvoiceRecipients.IsActive = 1";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { agrId = agreementId });

                return result;
            }
        }
    }

    
    public class MockedInternalEmployeesRepository : IInternalEmployeesRepository
    {
        public IEnumerable<InternalEmployee> GetAccountExecutivesThatShareBranchWithUserId(int id)
        {
            return new List<InternalEmployee>
            {
                new InternalEmployee
                {
                    Id = 1,
                    FirstName = "Judy",
                    LastName = "Winslow",
                    Title = "(AE)"
                },
                new InternalEmployee
                {
                    Id = 2,
                    FirstName = "Barbara",
                    LastName = "Franklin",
                    Title = "(AE)"
                },
                new InternalEmployee
                {
                    Id = 3,
                    FirstName = "Alice",
                    LastName = "Zoolander",
                    Title = "(MD)"
                }
            };
        }

        public IEnumerable<InternalEmployee> GetClientContactsWithCompanyId(int companyId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<InternalEmployee> GetBillingContactsWithCompanyId(int companyId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<InternalEmployee> GetDirectReportsWithCompanyId(int companyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InternalEmployee> GetInvoiceRecipientsWithAgreementId(int agreementId)
        {
            throw new NotImplementedException();
        }
    }
}
