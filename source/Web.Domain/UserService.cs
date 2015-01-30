using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain
{
    public class UserService
    {
        private readonly UserRepository _repository;
        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public User Find(int id)
        {
            return _repository.Find(id);
        }

        public User FindByName(string username)
        {
            return _repository.FindByName(username);
        }
    }
}
