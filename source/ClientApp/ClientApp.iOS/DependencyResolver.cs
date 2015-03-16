using System;
using Microsoft.Practices.Unity;
using ClientApp.Core;
using ClientApp.iOS.Startup;

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
            container.RegisterType<IHttpMessageHandlerFactory, NativeMessageHandlerFactory>();
            container.RegisterType<IErrorSource, ErrorSource>(new ContainerControlledLifetimeManager());
            container.RegisterType<IErrorReporter, ErrorReporter>();
            container.RegisterType<IMatchGuideApi, MatchGuideApi>();
            container.RegisterType<ITokenStore, TokenStore>();
            container.RegisterType<IActivityManager, ActivityManager>(new ContainerControlledLifetimeManager());
        }
    }
}