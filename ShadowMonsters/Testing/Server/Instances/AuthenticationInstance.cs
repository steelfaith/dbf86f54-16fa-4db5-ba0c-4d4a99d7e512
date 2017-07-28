using System;
using System.Linq;
using Common;
using Common.Interfaces;
using Common.Interfaces.Network;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Responses;
using NLog;
using Server.Common;
using Server.Common.Interfaces;

namespace Server.Instances
{
    public class AuthenticationInstance : IAuthenticationInstance
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConnectionManager _connectionManager;
        private readonly IUserController _userController;
        private readonly IWorldManager _worldManager;
        private int _clientIds;
        private object _clientLock = new object();

        public Guid InstanceId { get; }

        public AuthenticationInstance(IMessageHandlerRegistrar messageHandlerRegistrar, IWorldManager worldManager, IConnectionManager connectionManager, IUserController userController)
        {
            InstanceId = Guid.NewGuid();
            messageHandlerRegistrar.Register(OperationCode.ConnectRequest, Login);
            messageHandlerRegistrar.Register(OperationCode.SelectCharacterRequest, CharacterSelected);
            _connectionManager = connectionManager;
            _userController = userController;
            _worldManager = worldManager;
        }

        public void Login(RouteableMessage routeableMessage)
        {
            ConnectRequest request = routeableMessage.Message as ConnectRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            IClientConnection clientConnection;
            if (!_connectionManager.TryGetClientConnection(routeableMessage.TcpConnectionId, out clientConnection))
                Logger.Error($"Login failed for user {request.ClientId}, with connection id {routeableMessage.TcpConnectionId}");

            lock (_clientLock)
            {
                _clientIds++;
                request.ClientId = _clientIds;//TODO: remove this once we have some real auth in place
            }

            var user = new User(request.ClientId, routeableMessage.TcpConnectionId);
            _userController.AddUser(user);

            var results = _userController.GetCharacters(request.ClientId);

            //_connectionManager.Send(new RouteableMessage(routeableMessage.TcpConnectionId, new ConnectResponse { Id = request.Id, Characters = results.Select(x => x.Name).ToList() }));
            _userController.Send(request.ClientId, new ConnectResponse { ClientId = request.ClientId, Characters = results.Select(x => x.Name).ToList() });
        }
        public void CharacterSelected(RouteableMessage routeableMessage)
        {
            SelectCharacterRequest request = routeableMessage.Message as SelectCharacterRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");


            var currentRegion = _worldManager.GetCharacterRegion(request.ClientId);
            
            var character = new Character (1, request.ClientId) { WorldRegionInstance = currentRegion, CurrentPosition = new Vector3(),Name = request.CharacterName };
            currentRegion.SubscribeToRegion(character);

            _userController.Send(request.ClientId, new SelectCharacterResponse(request.ClientId, true));
        }

        public void Logout(RouteableMessage routeableMessage)
        {
            
        }
    }
}