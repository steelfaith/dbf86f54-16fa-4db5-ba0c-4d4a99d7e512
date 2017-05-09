﻿using System;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;
using Server.Common.Interfaces;

namespace Server.Instances
{
    public class BattleInstance : IBattleInstance
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IInstanceCoordinator _instanceCoordinator;
        private readonly IUserController _userController;

        public Guid InstanceId { get; }

        public BattleInstance(IMessageHandlerRegistrar messageHandlerRegistrar,
            IConnectionManager connectionManager, IInstanceCoordinator instanceCoordinator, IUserController userController)
        {
            InstanceId = Guid.NewGuid();
            messageHandlerRegistrar.Register(OperationCode.BattleInstanceRunRequest, AttemptRun);
            _connectionManager = connectionManager;
            _instanceCoordinator = instanceCoordinator;
            _userController = userController;
        }

        public void AttemptRun(RouteableMessage routeableMessage)
        {
            BattleInstanceRunRequest request = routeableMessage.Message as BattleInstanceRunRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = _userController.GetUserByClientId(request.ClientId);

            if (!user.BattleInstanceId.HasValue)
                throw new InvalidOperationException("User is not connected to a battle instance.");

            //var battleInstance = _instanceCoordinator.GetBattleInstance(user.BattleInstanceId.Value);

            var result = true; // need some implementation lol

            //_connectionManager.Send(new RouteableMessage(routeableMessage.ConnectionId, new BattleInstanceRunResponse { Successful = result, ClientId = user.ClientId }));
            user.ClientConnection.Send(new BattleInstanceRunResponse { Successful = result, ClientId = user.ClientId });
        }

        
    }
}