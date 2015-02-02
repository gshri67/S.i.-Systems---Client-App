﻿using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SiSystems.ClientApp.Web.Models;
using SiSystems.ClientApp.Web.Providers;

namespace SiSystems.ClientApp.Web.App_Start
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var userStore = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(UserStore)) as UserStore;

            var manager = new ApplicationUserManager(userStore)
            {
                //TODO: Replace this with a real password hasher that matches S.i. Systems' method.
                PasswordHasher = new PlainTextPasswordHasher()
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}