using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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
        private const int ThirtyFps = 33;//I know its 33 think about it ;) 

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IConnectionManager _connectionManager;
        private readonly IInstanceCoordinator _instanceCoordinator;
        private readonly IUserController _userController;
        private readonly Timer _eventProcessTimer;

        public Guid InstanceId { get; }

        public BattleInstance(IMessageHandlerRegistrar messageHandlerRegistrar,
            IConnectionManager connectionManager, IInstanceCoordinator instanceCoordinator,
            IUserController userController)
        {
            InstanceId = Guid.NewGuid();
            messageHandlerRegistrar.Register(new InstanceRoute(InstanceId, OperationCode.BattleInstanceRunRequest),
                AttemptRun);
            _connectionManager = connectionManager;
            _instanceCoordinator = instanceCoordinator;
            _userController = userController;
            _eventProcessTimer = new Timer(OnEventTimer,null, ThirtyFps, Timeout.Infinite);
        }

        private void OnEventTimer(object state)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                

                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                stopwatch.Stop();
                Logger.Info($"Battle instance {InstanceId} event timer took {stopwatch.Elapsed}");
                _eventProcessTimer.Change(ThirtyFps, Timeout.Infinite);
            }
        }

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

            _userController.Send(request.ClientId,
                new BattleInstanceRunResponse {Successful = result, ClientId = user.Id});
        }
    }
}