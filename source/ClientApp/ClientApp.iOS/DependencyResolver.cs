using System;
using ClientApp.Services;
using ClientApp.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace ClientApp.iOS
{
    public class DependencyResolver
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(CreateContainer);

        public static IUnityContainer Current { get { return Container.Value; } }

        private DependencyResolver()
        {
        }
        
        private static UnityContainer CreateContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<IEulaService, EulaService>();
            container.RegisterType<IConnectionService, ConnectionService>();
            container.RegisterType<IAlumniService, AlumniService>();
        }
    }
}