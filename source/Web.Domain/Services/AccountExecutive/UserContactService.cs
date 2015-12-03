﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class UserContactService
    {
        private readonly IUserContactRepository _repo;
        private readonly ISessionContext _context;

        public UserContactService(IUserContactRepository repo, ISessionContext context)
        {
            _repo = repo;
            _context = context;
        }

        public UserContact GetUserContactById(int id)
        {
            var contact = _repo.GetUserContactById(id);

            return contact;
        }
    }
}