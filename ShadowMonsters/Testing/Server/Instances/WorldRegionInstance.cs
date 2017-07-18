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

        public Guid InstanceId { get; }
        private readonly ConcurrentDictionary<int, Character> _characters = new ConcurrentDictionary<int, Character>();

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

        public void SubscribeToRegion(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            _characters[character.UserId] = character;

            Broadcast(new PlayerConnectedEvent());
        }

        public void Move(RouteableMessage routeableMessage)
        {
            PlayerMoveRequest request = routeableMessage.Message as PlayerMoveRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            var user = UserController.GetUserByClientId(request.ClientId);

            MovePlayer(user.Id, request.Position);
        }

        public void MovePlayer(int userId, Vector3 newPosition)
        {
            Character character;
            if (!_characters.TryGetValue(userId, out character))
                return;

            character.NextPosition = newPosition;
            //validate new postion after looking up character
        }

        private void Tick(object state)
        {
            try
            {
                Dictionary<int, Vector3> updatedPositions = new Dictionary<int, Vector3>();

                foreach (var character in _characters.Values)
                {
                    if (character.NextPosition != null && !character.CurrentPosition.Equals(character.NextPosition ))
                    {
                        character.CurrentPosition = character.NextPosition.Value;
                        character.NextPosition = null;

                        updatedPositions.Add(character.UserId, character.CurrentPosition);//only add to our list if we actually have a change
                    }
                }
                
                if(updatedPositions.Count > 0)
                    Broadcast(new PlayerMoveEvent(updatedPositions));
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

        private void Broadcast(Message message)
        {
            try
            {
                Parallel.ForEach(_characters.Values, (character) =>
                {
                    var user = UserController.GetUserByClientId(character.UserId);
                    message.ClientId = user.Id;
                    user.ClientConnection.Send(message);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }
    }
}