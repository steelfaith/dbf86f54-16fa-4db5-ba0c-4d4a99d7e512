using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Messages.Events;
using Common.Messages.Requests;
using log4net;
using Microsoft.Practices.Unity;
using Server.Common;
using Server.Common.Interfaces;

namespace Server.Instances
{
    /// <summary>
    /// Represent a physical area a player can be located in, currently they do not support
    /// stacking or player separation by population however this is comething we should 
    /// add in the future. In the interim world regions are used as a low grade
    /// region of interest filter
    /// </summary>
    public class WorldRegionInstance : IWorldRegionInstance
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConnectionManager));
        private readonly BoundingBox _boundingBox;
        private readonly Timer _regionTick;
        private readonly IMessageHandlerRegistrar _messageHandlerRegistrar;
        private readonly IUserController _userController;

        public Guid InstanceId { get; }

        private readonly ConcurrentDictionary<int, User> _users = new ConcurrentDictionary<int, User>();

        [Dependency]
        public IUserController UserController { get; set; }

        [Dependency]
        public IConnectionManager ConnectionManager { get; set; }

        public WorldRegionInstance(IMessageHandlerRegistrar messageHandlerRegistrar, IUserController userController)
        {
            InstanceId = Guid.NewGuid();
            _boundingBox = new BoundingBox();
            messageHandlerRegistrar.Register(OperationCode.PlayerMoveRequest, Move);
            _regionTick = new Timer(Tick, null, 100, Timeout.Infinite);
        }

        public void SubscribeToRegion(User user)
        {
            if (user.ActiveCharacter == null)
                throw new ArgumentNullException(nameof(user));

            _users[user.ClientId] = user;
        }

        public void Move(RouteableMessage routeableMessage)
        {
            PlayerMoveRequest request = routeableMessage.Message as PlayerMoveRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = _userController.GetUserByClientId(request.ClientId);

            MovePlayer(user.ClientId, request.Position);
        }

        public void MovePlayer(int userId, Vector3 newPosition)
        {
            User user;
            if (!_users.TryGetValue(userId, out user))
                return;

            user.ActiveCharacter.NextPosition = newPosition;
            //validate new postion after looking up character
            //eventually double check here with an accumulator 

        }

        private void Tick(object state)
        {
            try
            {
                Dictionary<int, Vector3> updatedPositions = new Dictionary<int, Vector3>();

                foreach (var user in _users.Values)
                {
                    user.ActiveCharacter.CurrentPosition = user.ActiveCharacter.NextPosition.GetValueOrDefault();
                    user.ActiveCharacter.NextPosition = null;

                    updatedPositions.Add(user.ClientId, user.ActiveCharacter.CurrentPosition);
                }

                Parallel.ForEach(_users.Values, (user) =>
                {
                    user.ClientConnection.Send(new PlayerMoveEvent(user.ClientId, updatedPositions));
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                _regionTick.Change(100, Timeout.Infinite);
            }

        }
    }
}