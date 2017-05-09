using System;
using System.Linq;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Common.Networking.Sockets;
using Server.Common;
using Server.Common.Interfaces;

namespace Server.Instances
{
    public class AuthenticationInstance : IAuthenticationInstance
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IUserController _userController;

        public Guid InstanceId { get; }

        public AuthenticationInstance(IMessageHandlerRegistrar messageHandlerRegistrar, IConnectionManager connectionManager, IUserController userController)
        {
            InstanceId = Guid.NewGuid();
            messageHandlerRegistrar.Register(OperationCode.ConnectRequest, Login);
            messageHandlerRegistrar.Register(OperationCode.SelectCharacterRequest, CharacterSelected);
            _connectionManager = connectionManager;
            _userController = userController;
        }

        public void Login(RouteableMessage routeableMessage)
        {
            ConnectRequest request = routeableMessage.Message as ConnectRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var connection = _connectionManager.GetClientConnection(routeableMessage.ConnectionId);

            if(connection == null)
                throw new ArgumentException("Failed to find client connection.");

            var user = new User(request.ClientId, connection);

            _userController.AddUser(user);

            var results = _userController.GetCharacters(request.ClientId);

            //_connectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new ConnectResponse { ClientId = request.ClientId, Characters = results.Select(x => x.Name).ToList() }));
            user.ClientConnection.Send(new ConnectResponse { ClientId = request.ClientId, Characters = results.Select(x => x.Name).ToList() });
        }
        public void CharacterSelected(RouteableMessage routeableMessage)
        {
            SelectCharacterRequest request = routeableMessage.Message as SelectCharacterRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = _userController.GetUserByClientId(request.ClientId);

            //var currentRegion = WorldManager.GetCharacterRegion(request.ClientId);

            //currentRegion.SubscribeToRegion(user);
            //user.ActiveCharacter = new Character { WorldRegionInstance = currentRegion, CurrentPosition = new Vector3(), UserId = user.ClientId, Name = request.CharacterName };

            //_connectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new SelectCharacterResponse(request.ClientId, true)));
            user.ClientConnection.Send(new SelectCharacterResponse(request.ClientId, true));
        }

        public void Logout(RouteableMessage routeableMessage)
        {
            
        }
    }
}