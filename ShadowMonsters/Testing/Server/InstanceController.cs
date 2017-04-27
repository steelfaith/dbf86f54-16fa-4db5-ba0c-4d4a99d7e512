using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Networking;
using Common.Networking.Sockets;
using Microsoft.Practices.Unity;
using Server.Common.Handlers.Requests;
using Server.Common.Interfaces;
using Server.Storage;
using Server.Storage.Providers;

namespace Server
{
    public class InstanceController : IInstanceCoordinator
    {
        private readonly IUnityContainer _container = new UnityContainer();
        private readonly ConcurrentDictionary<Guid,BattleInstance> _battleInstances = new ConcurrentDictionary<Guid, BattleInstance>();
        

        public InstanceController()
        {
            _container.RegisterType<MessageDispatcher>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMessageHandlerRegistrar, MessageHandlerRegistrar>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITcpConnectionManager, TcpConnectionManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IInstanceCoordinator, InstanceController>();
            _container.RegisterType<AsyncSocketListener>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ConnectRequestHandler>(new ContainerControlledLifetimeManager());
            _container.RegisterType<CreateBattleInstanceRequestHandler>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserController, UserController>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUsersStorageProvider, UsersStorageProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IBattleInstanceStorageProvider, BattleInstanceStorageProvider>(new ContainerControlledLifetimeManager());
        }

        public void Start()
        {
            AsyncSocketListener listener = new AsyncSocketListener(_container.Resolve<ITcpConnectionManager>(), _container.Resolve<MessageDispatcher>(), _container.Resolve<IMessageHandlerRegistrar>());
            _container.Resolve<ConnectRequestHandler>();
            _container.Resolve<CreateBattleInstanceRequestHandler>();
            Task.Run(() => listener.StartListening());
        }


        public Guid CreateInstance()
        {
            BattleInstance instance = new BattleInstance();
            _battleInstances.TryAdd(instance.InstanceRoutingId, instance);

            return instance.InstanceRoutingId;
        }

        public IBattleInstance GetBattleInstance(Guid instanceId)
        {
            BattleInstance instance;
            _battleInstances.TryGetValue(instanceId, out instance);
            return instance;
        }

    }
}