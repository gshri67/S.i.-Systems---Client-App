using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Practices.Unity;
using Foundation;
using UIKit;

namespace App2
{
    class DependencyResolver
    {/*
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
            container.RegisterType<IActivityManager, ActivityManager>(new ContainerControlledLifetimeManager());

            // instantiate the error reporter so that it registers itself with the error source
            var errorReporter = new ErrorReporter(container.Resolve<IErrorSource>());
        }*/
    }
}