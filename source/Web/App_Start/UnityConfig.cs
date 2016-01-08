using System;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.Web.Auth;
using SiSystems.ClientApp.Web.Caching;
using SiSystems.ClientApp.Web.Domain.Caching;
using SiSystems.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;
using System.Net.Http;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ClientApp.Web.Domain.Services.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ISessionContext, SessionContext>(new PerRequestLifetimeManager());
            container.RegisterType<IDateTimeService, DateTimeService>();

            container.RegisterType<IConsultantRepository, ConsultantRepository>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<IClientDetailsRepository, ClientDetailsRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ITimesheetRepository, TimesheetRepository>();
            container.RegisterType<ITimeEntryRepository, TimeEntryRepository>();
            container.RegisterType<IRemittanceRepository, RemittanceRepository>();
            container.RegisterType<IConsultantDetailsRepository, ConsultantDetailsRepository>();
            container.RegisterType<IDirectReportRepository, DirectReportRepository>();
            container.RegisterType<IActivityRepository, ActivityRepository>();
            container.RegisterType<IPayRateRepository, PayRateRepository>();
            container.RegisterType<IJobsRepository, JobsRepository>();
            container.RegisterType<IContractorRepository, ContractorRepository>();
            container.RegisterType<IContractorRateRepository, ContractorRateRepository>();
            container.RegisterType<IUserContactRepository, UserContactRepository>();
            container.RegisterType<IObjectCache, ObjectCache>();
            container.RegisterType<HttpMessageHandler, HttpClientHandler>();
            
            container.RegisterType<IConsultantContractRepository, ConsultantContractRepository>();
        }
    }
}
