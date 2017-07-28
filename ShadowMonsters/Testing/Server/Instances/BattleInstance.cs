using System;
using Common;
using Common.Interfaces;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Responses;
using NLog;
using Server.Common;
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
            messageHandlerRegistrar.Register(new InstanceRoute(InstanceId,OperationCode.BattleInstanceRunRequest), AttemptRun);
            _connectionManager = connectionManager;
            _instanceCoordinator = instanceCoordinator;
            _userController = userController;
        }

        //now we need to figure some things out, the battle instance shouldnt register for all requests,
        //they need to be routed based on user.BattleInstanceId which makes things complicated. The other
        //possible solution is that we should spin up a new connection for every battle instance, and the response
        //message from the instance coordinator should give us the address of the new instance

        public void AttemptRun(InstanceMessage instanceMessage)
        {
            BattleInstanceRunRequest request = instanceMessage as BattleInstanceRunRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            User user;
            if (!_userController.TryGetUserByClientId(request.ClientId, out user))
                return;

            if (!user.BattleInstanceId.HasValue)
                throw new InvalidOperationException("User is not connected to a battle instance.");


            var result = true; // need some implementation lol

            _userController.Send(request.ClientId, new BattleInstanceRunResponse { Successful = result, ClientId = user.Id });
        }

        
    }
}