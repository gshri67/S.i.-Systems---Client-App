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
    }
}
