using Common;
using Common.Networking.Sockets;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;
using Server.Instances;
using Server.Storage;
using Server.Storage.Providers;

namespace Server
{
    public static class UnityRegistrar
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        static UnityRegistrar()
        {
            Container.RegisterType<IMessageDispatcher, MessageDispatcher>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IMessageHandlerRegistrar, MessageHandlerRegistrar>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IConnectionManager, ConnectionManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUserController, UserController>(new ContainerControlledLifetimeManager());
            Container.RegisterType<AsyncSocketListener>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IWorldManager, WorldManager>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IInstanceCoordinator, InstanceCoordinator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBattleInstanceStorageProvider, BattleInstanceStorageProvider>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUsersStorageProvider, UsersStorageProvider>(new ContainerControlledLifetimeManager());

            
            
        }

        public static IUnityContainer GetUnityContainer()
        {
            return Container;
        }
    }
}