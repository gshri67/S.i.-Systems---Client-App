﻿using SiSystems.ClientApp.Web.Domain.Repositories;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Services
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

        public User FindByPrimaryEmail(string username)
        {
            return _repository.FindByPrimaryEmail(username);
        }
    }
}
