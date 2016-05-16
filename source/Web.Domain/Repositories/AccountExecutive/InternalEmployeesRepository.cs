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
        IEnumerable<InternalEmployee> GetAccountExecutivesWithBranch(int branch);
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

        public IEnumerable<InternalEmployee> GetAccountExecutivesWithBranch(int branch)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractSummaryQuery =
                    @"select users.UserId as Id, users.FirstName, users.LastName, users.Title 
                    from   users inner join user_rolematrix on user_rolematrix.userid = users.userid
                    left join user_office on user_office.userofficeid = users.userofficeid
                    left join verticaldetails on verticaldetails.verticalid=user_office.verticalid
                    inner join user_role on user_role.UserRoleID =user_rolematrix.UserRoleID
                    inner join (
                                  select 
                                  max(user_role.rank) as rank,
                                  users.userid
                                  from          
                                  user_role
                                  inner join user_rolematrix u on u.userroleid = user_role.userroleid
                                  inner join users on users.userid = u.userid
                                  and user_role.inactive = 0
                                  and    users.inactive = 0
                                  and user_role.title in ('AccountExecutive','ManagingDirector')
                                  group by users.userid
                                  )as user_roleText 
                    on user_roleText.rank =user_role.rank 
                    and user_roleText.userid = users.userid
                    where  user_rolematrix.userroleid in
                                  (
                                         select userroleid 
                                               from   user_role
                                               where  title = 'AccountExecutive' or title = 'ManagingDirector'
                                               and    inactive = 0
                                  )
                    and    users.userofficeid IN (@Branch)";

                var employees = db.Connection.Query<InternalEmployee>(contractSummaryQuery, new { Branch = branch });

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

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyid = companyId });

                return result;
            }
        }

        public IEnumerable<InternalEmployee> GetBillingContactsWithCompanyId(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"select  distinct
                                       users.userid as Id,
                                       users.firstname as FirstName,
                                       users.lastname as LastName,
                                       users.firstname + ' ' + users.lastname as contactfullname

                       from    users
                                       left join company on (company.companyid = users.companyid and company.Inactive = 0)
                                       left join user_email on user_email.userid = users.userid
                                       inner join (   select user_address.userid,
                                                                                     count(address.addressid) as addcnt
                                                                     from
                                                                     user_address 
                                                                     inner join address on (user_address.addressid = address.addressid and address.inactive = 0)
                                                                     inner join picklist on (address.addresstype = picklist.picklistid and picklist.inactive = 0)
                                                                     where picklist.title= 'Billing Address' and user_address.inactive = 0
                                                                     group by user_address.userid ) as Billingaddcount
                                       on (Billingaddcount.userid=users.userid and addcnt = 1)
                       where   
                               
                                              users.companyid in ( select companyid from dbo.[UDF_GetAllDivisionsForCompany](@companyid)) 
                                      
                                       and users.usertype =
                                              ( 
                                              select picklist.picklistid
                                              from    picklist
                                              where   picklist.picktypeid =
                                                      (
                                                      select  picktypeid
                                                      from    picktype
                                                      where   type = 'UserRoles'
                                                      )
                                              and     picklist.inactive = 0
                                              and     title = 'Client Contact'
                                              )
                                       and     users.inactive = 0

                order by
                        users.firstname,
                       users.lastname";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyid = companyId });

                return result;
            }
        }

        public IEnumerable<InternalEmployee> GetDirectReportsWithCompanyId(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"
                    SELECT 
                           RESULTRECORDSET_DR.contactuserid as Id,
                           RESULTRECORDSET_DR.firstname as FirstName,
                           RESULTRECORDSET_DR.lastname as LastName
                    FROM    
                           (select 
                                           distinct
                                           users.userid as contactuserid,
                                           users.firstname,
                                           users.lastname,
                                           users.firstname + ' ' + users.lastname as contactfullname,
                                           (case 
                                                  when user_email.primaryemail is null
                                                  then '-'
                                                  else user_email.primaryemail
                                           end)
                                           primaryemail,
                                           company.companyname
                           from    users
                                           inner join company on company.companyid = users.companyid
                                           left join user_email on users.userid = user_email.userid
                           where   
                               
                                                  users.companyid in ( select companyid from dbo.[UDF_GetAllDivisionsForCompany](@companyid)) 
                               
                               
                                           and users.usertype =
                                                  ( 
                                                  select picklist.picklistid
                                                  from    picklist
                                                  where   picklist.picktypeid =
                                                          (
                                                          select picktypeid
                                                          from    picktype
                                                          where   type = 'UserRoles'
                                                          )
                                                  and     picklist.inactive = 0
                                                  and     title = 'Client Contact'
                                                  )
                                           and     users.inactive = 0
                               
                                           ) AS RESULTRECORDSET_DR
                     order by 
                                   RESULTRECORDSET_DR.firstname,
                                   RESULTRECORDSET_DR.LastName";

                var result = db.Connection.Query<InternalEmployee>(contractsQuery, param: new { companyid = companyId });

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

        public IEnumerable<InternalEmployee> GetAccountExecutivesWithBranch(int branch)
        {
            throw new NotImplementedException();
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
