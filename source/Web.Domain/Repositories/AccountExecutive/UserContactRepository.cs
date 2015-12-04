using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IUserContactRepository
    {
        UserContact GetUserContactById(int id);
    }

    public class UserContactRepository : IUserContactRepository
    {
        public UserContact GetUserContactById(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class MockUserContactRepository : IUserContactRepository
    {
        public UserContact GetUserContactById(int id)
        {
            return new UserContact
            {
                Id = 1,
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
