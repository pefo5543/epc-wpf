using EpcDashboard.Services;
using EpcDashboard.Services.Interfaces;
using Microsoft.Practices.Unity;
using System;

namespace EpcDashboard.MVVMHelpers
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;
        static ContainerHelper()
        {
            try
            {
                //all the objects that will be asked from the viewmodels should be specified here
                //example: a viewmodel asks for a ICustomersRepository, ContainerHelper returns a CustomersRepository object
                _container = new UnityContainer();
                _container.RegisterType<IMainRepository, MainRepository>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IMainWindowViewModel, MainWindowViewModel>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ICopyService, CopyService>(new ContainerControlledLifetimeManager());
                //ContainerControlledLifetimeManager = Unity will try to make the CustomerRepository singleton
            }
            catch (Exception e)
            {
                Console.WriteLine("UnityContainer exception: {0}", e.Message);
            }
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
