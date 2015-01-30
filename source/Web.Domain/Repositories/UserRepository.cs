using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class UserRepository
    {

        private static readonly List<User> Users = new List<User>
        {
            new User
            {
                Id=1, ClientId=1,
                FirstName = "Bob", LastName = "Smith", 
                EmailAddress = "bob.smith@email.com", PasswordHash = "password"
            }
        };

        public User Find(int id)
        {
            return Users.SingleOrDefault(u => u.Id == id);
        }

        public User FindByName(string username)
        {
            return Users.SingleOrDefault(u => u.EmailAddress == username);
        }
    }
}
