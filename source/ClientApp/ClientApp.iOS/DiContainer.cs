using ClientApp.Services;
using ClientApp.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace ClientApp.iOS
{
    public class DiContainer
    {
        private readonly IUnityContainer _container;
        public IUnityContainer Instance { get { return _container; } }

        public DiContainer()
        {
            _container = new UnityContainer();

            RegisterTypes();
        }

        private void RegisterTypes()
        {
            _container.RegisterType<ILoginService, LoginService>();
            _container.RegisterType<IEulaService, EulaService>();
        }
    }
}