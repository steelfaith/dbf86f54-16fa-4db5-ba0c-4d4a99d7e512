using System;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;
using IUserController = Server.Common.Interfaces.IUserController;

namespace Server.Common.Handlers.Requests
{
    public class BattleInstanceRunRequestHandler : IMessageHandler
    {
        public OperationCode OperationCode => OperationCode.BattleInstanceRunRequest;

        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly IInstanceCoordinator _instanceCoordinator;
        private readonly ITcpConnectionManager _tcpConnectionManager;
        private readonly IUserController _userController;

        [InjectionConstructor]
        public BattleInstanceRunRequestHandler(IMessageHandlerRegistrar messageHandlerRegistrar,
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
            BattleInstanceRunRequest request = routeableMessage.Message as BattleInstanceRunRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = _userController.GetUserByConnectionId(routeableMessage.ConnectionId);

            if (!user.BattleInstanceId.HasValue)
                throw new InvalidOperationException("User is not connected to a battle instance.");

            var battleInstance = _instanceCoordinator.GetBattleInstance(user.BattleInstanceId.Value);

            var result = battleInstance.AttemptRun();

            _tcpConnectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new BattleInstanceRunResponse { Successful = result, ClientId = user.ClientId}));
        }
    }
}