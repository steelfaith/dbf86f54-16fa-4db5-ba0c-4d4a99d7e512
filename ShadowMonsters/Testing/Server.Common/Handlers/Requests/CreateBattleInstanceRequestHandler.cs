using System;
using Common;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;
using IUserController = Server.Common.Interfaces.IUserController;

namespace Server.Common.Handlers.Requests
{
    public class CreateBattleInstanceRequestHandler : IMessageHandler
    {
        public OperationCode OperationCode => OperationCode.CreateBattleInstanceRequest;

        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly IInstanceCoordinator _instanceCoordinator;
        private readonly ITcpConnectionManager _tcpConnectionManager;
        private readonly IUserController _userController;


        [InjectionConstructor]
        public CreateBattleInstanceRequestHandler(IMessageHandlerRegistrar messageHandlerRegistrar, 
            ITcpConnectionManager tcpConnectionManager, IInstanceCoordinator instanceCoordinator, IUserController userController)
        {
            _messageHandlerRegistrar = messageHandlerRegistrar;
            _messageHandlerRegistrar.Register(this);
            _tcpConnectionManager = tcpConnectionManager;
            _instanceCoordinator = instanceCoordinator;
            _userController = userController;
        }
        public void HandleMessage(RouteableMessage routeableMessage)
        {
            CreateBattleInstanceRequest request = routeableMessage.Message as CreateBattleInstanceRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user =_userController.GetUserByConnectionId(routeableMessage.ConnectionId);

            if(user.BattleInstanceId.HasValue)
                throw new InvalidOperationException("User is already connected to a battle instance.");

            user.BattleInstanceId = _instanceCoordinator.CreateInstance();

            _tcpConnectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new CreateBattleInstanceResponse { ClientId = user.ClientId } ));
        }
    }
}