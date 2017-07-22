using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Messages.Requests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Server.Common.Interfaces;
using Server.Networking.Sockets;

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

            var worldRegion = instanceCoordinator.CreateWorldRegion();
            _regions[worldRegion.InstanceId] = worldRegion;
        }

        public void Start()
        {
            Task.Run(() => _asyncSocketListener.StartListening());
        }

        public IWorldRegionInstance GetCharacterRegion(int clientId)
        {
            var region = _regions.Values.First();//temporary we need to calculate this in the future
            return region;    
        }

        public void OnBuiltUp(NamedTypeBuildKey buildKey)
        {
            var authInstance = _instanceCoordinator.CreateAuthenticationInstance();
            _authInstances[authInstance.InstanceId] = authInstance;
        }

        public void OnTearingDown()
        {

        }
    }
}