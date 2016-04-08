using System;
using Microsoft.Practices.Unity;
using ClientApp.iOS.Startup;
using Shared.Core;
using Shared.Core.Platform;

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
            container.RegisterType<IMatchGuideApi, MatchGuideApi>();
            container.RegisterType<ITokenStore, TokenStore>();
            container.RegisterType<IDefaultStore, DefaultStore>();
            container.RegisterType<IActivityManager, ActivityManager>(new ContainerControlledLifetimeManager());

            // instantiate the error reporter so that it registers itself with the error source
            var errorReporter = new ErrorReporter(container.Resolve<IErrorSource>());
        }
    }
}