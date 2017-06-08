using System;
using System.Collections.Concurrent;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;

namespace Server.Instances
{
    public class InstanceCoordinator : IInstanceCoordinator
    {
        private readonly IUnityContainer _container;
        private readonly ConcurrentDictionary<Guid,IBattleInstance> _battleInstances = new ConcurrentDictionary<Guid, IBattleInstance>();
        private readonly IUserController _userController;
        private readonly IConnectionManager _connectionManager;

        [InjectionConstructor]
        public InstanceCoordinator(IUnityContainer container, IMessageHandlerRegistrar messageHandlerRegistrar, IUserController userController, IConnectionManager connectionManager)
        {
            _container = container;
            _container.RegisterType<IAuthenticationInstance, AuthenticationInstance>(new PerResolveLifetimeManager());
            _container.RegisterType<IBattleInstance, BattleInstance>(new PerResolveLifetimeManager());
            _container.RegisterType<IWorldRegionInstance, WorldRegionInstance>(new PerResolveLifetimeManager());

            messageHandlerRegistrar.Register(OperationCode.CreateBattleInstanceResponse, CreateBattleInstance);
            _userController = userController;
            _connectionManager = connectionManager;
        }
        
        public Guid CreateBattleInstance()
        {
            IBattleInstance instance = _container.Resolve<IBattleInstance>();

            _battleInstances[instance.InstanceId] = instance;

            return instance.InstanceId;
        }

        public void CreateBattleInstance(RouteableMessage routeableMessage)
        {
            CreateBattleInstanceRequest request = routeableMessage.Message as CreateBattleInstanceRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = _userController.GetUserByClientId(request.ClientId);

            if (user.BattleInstanceId.HasValue)
                throw new InvalidOperationException("User is already connected to a battle instance.");

            user.BattleInstanceId = CreateBattleInstance();

            //_connectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new CreateBattleInstanceResponse { Id = user.Id }));
            user.ClientConnection.Send(new CreateBattleInstanceResponse { ClientId = user.Id });
        }

        public IBattleInstance GetBattleInstance(Guid instanceId)
        {
            IBattleInstance instance;
            _battleInstances.TryGetValue(instanceId, out instance);
            return instance;
        }

        public IWorldRegionInstance CreateWorldRegion()
        {
            return _container.Resolve<IWorldRegionInstance>();
        }

        public IAuthenticationInstance CreateAuthenticationInstance()
        {
            return _container.Resolve<IAuthenticationInstance>();
        }

    }
}