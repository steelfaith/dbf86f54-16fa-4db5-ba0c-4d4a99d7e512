using System;
using System.Linq;
using Common;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;
using Server.Networking.Sockets;

namespace Server.Common.Handlers.Requests
{
    /// <summary>
    /// we probably want a single instance of these handlers
    /// but how do we handle the server interaction logic /threading 
    /// model 
    /// </summary>
    public class ConnectRequestHandler : IMessageHandler
    {
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly ITcpConnectionManager _tcpConnectionManager;
        private readonly IUserController _userController;

        [InjectionConstructor]
        public ConnectRequestHandler(IMessageHandlerRegistrar messageHandlerRegistrar, ITcpConnectionManager connectionManager, IUserController userController)
        {
            _messageHandlerRegistrar = messageHandlerRegistrar;
            _messageHandlerRegistrar.Register(this);
            _tcpConnectionManager = connectionManager;
            _userController = userController;
        }
        public OperationCode OperationCode => OperationCode.ConnectRequest;

        public void HandleMessage(RouteableMessage routeableMessage)
        {
            ConnectRequest request = routeableMessage.Message as ConnectRequest;

            if(request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            _userController.AddUser(new User(request.ClientId, routeableMessage.ConnectionId));

            var results = _userController.GetCharacters(request.ClientId);

            _tcpConnectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new ConnectResponse {
                                                                                                                    ClientId = request.ClientId,
                                                                                                                    Characters = results.Select(x => x.Name).ToList(),
                                                                                                                    Announcement = new ServerAnnouncement("Welcome to fucking Shadow Monsters from the real server!!!"),
                                                                                                                }));
        }
    }
}