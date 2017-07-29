using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;
using Common.Messages;
using Common.Messages.Events;
using Common.Messages.Requests;
using Microsoft.Practices.Unity;
using NLog;
using Server.Common;
using Server.Common.Interfaces;

namespace Server.Instances
{
    /// <summary>
    /// Represent a physical area a player can be located in, currently they do not support
    /// stacking or player separation by population however this is something we should 
    /// add in the future. In the interim world regions are used as a low grade
    /// region of interest filter
    /// </summary>
    public class WorldRegionInstance : IWorldRegionInstance
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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

            _characters[character.ClientId] = character;

            Broadcast(new PlayerConnectedEvent());
        }

        public void Move(RouteableMessage routeableMessage)
        {

            PlayerMoveRequest request = routeableMessage.Message as PlayerMoveRequest;

            if (request == null)
                throw new ArgumentException("Failed to convert message to appropriate handler type.");

            User user;
            if (!UserController.TryGetUserByClientId(request.ClientId, out user))
                return;

            Logger.Info("Received a player move request from client: {0} with new location {1},{2},{3}", user.Id, request.Position.X, request.Position.Y, request.Position.Z);

            MovePlayer(user.Id, request.Position, request.Forward);
        }

        public void MovePlayer(int userId, Vector3 newPosition, Vector3 forward)
        {
            Character character;
            if (!_characters.TryGetValue(userId, out character))
                return;

            character.NextPosition = newPosition;
            //validate new postion after looking up character
            //also in the future if we throw out the position update discard the forward changes as well
            character.Forward = forward;
        }

        private void Tick(object state)
        {
            try
            {
                Dictionary<int, PositionForwardTuple> updatedPositions = new Dictionary<int, PositionForwardTuple>();

                foreach (var character in _characters.Values)
                {
                    if (character.NextPosition != null && !character.CurrentPosition.Equals(character.NextPosition ))
                    {
                        character.CurrentPosition = character.NextPosition.Value;
                        character.NextPosition = null;
                        updatedPositions.Add(character.ClientId,
                                new PositionForwardTuple {Position = character.CurrentPosition, Forward = character.Forward});
                            //only add to our list if we actually have a change
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
                    var isConnected = UserController.Send(character.ClientId, message);
                    if (!isConnected)
                    {
                        Character outCharacter;
                        if(_characters.TryRemove(character.ClientId, out outCharacter))
                            Logger.Warn($"Removed disconnected character {character.ClientId}");
                        else
                            Logger.Error($"Failed to remove character {character.ClientId}");
                            
                    }
                        
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }


    }
}