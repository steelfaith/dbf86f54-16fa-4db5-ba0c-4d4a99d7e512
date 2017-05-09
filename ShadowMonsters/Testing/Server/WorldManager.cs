using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Networking.Sockets;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;

namespace Server
{
    public class WorldManager : IWorldManager
    {
        private readonly IUnityContainer _container = new UnityContainer();
        private readonly ConcurrentDictionary<Guid, IWorldRegionInstance> _regions = new ConcurrentDictionary<Guid, IWorldRegionInstance>();
        private readonly ConcurrentDictionary<Guid, IAuthenticationInstance> _authInstances = new ConcurrentDictionary<Guid, IAuthenticationInstance>();
        private readonly IInstanceCoordinator _instanceCoordinator;
        private readonly AsyncSocketListener _asyncSocketListener;

        [InjectionConstructor]
        public WorldManager(IInstanceCoordinator instanceCoordinator, AsyncSocketListener asyncSocketListener)
        {
            _asyncSocketListener = asyncSocketListener;
            _instanceCoordinator = instanceCoordinator;

            var authInstance = instanceCoordinator.CreateAuthenticationInstance();
            _authInstances[authInstance.InstanceId] = authInstance;

            var worldRegion = instanceCoordinator.CreateWorldRegion();
            _regions[worldRegion.InstanceId] = worldRegion;
        }

        public void Start()
        {
            Task.Run(() => _asyncSocketListener.StartListening());
        }

        public IWorldRegionInstance GetCharacterRegion(int userId)
        {
            var region = _regions.Values.First();//temporary lol
            return region;    
        }
    }
}