using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Common;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Responses;
using NLog.LayoutRenderers.Wrappers;
using UnityEngine;

namespace Assets.Scripts.NetworkAgents
{
    public class AuthenticationAgent : MonoBehaviour
    {
        public ClientConnectionManager ConnectionManager;
        private readonly Dictionary<string, NetworkResponseAction> _consumers = new Dictionary<string, NetworkResponseAction>();
        private readonly Queue<NetworkResponseAction> _actionQueue = new Queue<NetworkResponseAction>();

        private readonly object _consumerLock = new object();

        public bool LoginSuccessful;

        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            //dequeuing an event on the main thread is potentially a performance problem in the future,
            //however it seems to be the cleanest way of handling events and I'm not sure that its any
            //worse/less contentious than a coroutine
            NetworkResponseAction networkResponse = null;
            lock (_consumerLock)
            {
                if (_actionQueue.Count > 0)
                    networkResponse = _actionQueue.Dequeue();
            }

            if(networkResponse != null)
                networkResponse.Action.Invoke(networkResponse.Message);

            //anything else it needs to do 
        }

        public void Login()
        {
            ConnectionManager.SendMessage(new ConnectRequest());
        }

        public void HandleConnectionResponse(RouteableMessage routeableMessage)
        {
            ConnectResponse response = routeableMessage.Message as ConnectResponse;

            if (response == null)
                return;

            ConnectionManager.ClientId = response.ClientId;

            lock (_consumerLock)
            {
                foreach (var item in _consumers.Values)
                    if (item.MessageType == typeof(ConnectResponse))
                    {
                        item.Message = response;
                        _actionQueue.Enqueue(item);
                    }
                        
            }

            ConnectionManager.SendMessage(new SelectCharacterRequest("BMoney!"));
        }

        public void RegisterForResponse(string scriptName, NetworkResponseAction action)
        {
            lock (_consumerLock)
            {
                if (!_consumers.ContainsKey(scriptName))
                    _consumers.Add(scriptName, action);
                else
                    Debug.LogErrorFormat("{0} attmpted to double register for a response", scriptName);
            }

        }

        public void UnregisterForResponse(string scriptName, NetworkResponseAction action)
        {
            lock (_consumerLock)
            {
                if (_consumers.ContainsKey(scriptName))
                    _consumers.Remove(scriptName);
                else
                    Debug.LogErrorFormat("{0} attmpted to double unregister for a response it wasnt registered for", scriptName);
            }
        }



        public void HandleSelectCharacterResponse(RouteableMessage routeableMessage)
        {
            SelectCharacterResponse response = routeableMessage.Message as SelectCharacterResponse;

            if (response == null)
                return;

            //ummm do something i guess no idea what yet
        }

    }
}
