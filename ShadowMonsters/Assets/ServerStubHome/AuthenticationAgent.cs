
using UnityEngine;
using Assets.Scripts;
using Common.Messages;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Messages.Requests;
using Common.Messages.Responses;

namespace Assets.ServerStubHome
{
    public class AuthenticationAgent : MonoBehaviour
    {
        private TextLogDisplayManager _textLogDisplayManger;
        private static AuthenticationAgent _authenticationAgent;
        private Queue<ServerAnnouncement> _serverWelcomeQueue = new Queue<ServerAnnouncement>();
        private ClientConnectionManager _connectionManager;
        public bool LoginSuccessful;

        private void Awake()
        {
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        private void Start()
        {                       
            _textLogDisplayManger = TextLogDisplayManager.Instance();
        }

        public void Login()
        {
            _connectionManager.SendMessage(new ConnectRequest());
        }

        private void Update()
        {
            if (_serverWelcomeQueue.Count > 0)
                StartCoroutine(CheckForMessageUpdates());
        }

        public void HandleConnectionResponse(RouteableMessage routeableMessage)
        {
            ConnectResponse response = routeableMessage.Message as ConnectResponse;

            if (response == null)
                return;

            LoginSuccessful = true;

            _serverWelcomeQueue.Enqueue(response.Announcement);
            _connectionManager.ClientId = response.ClientId;

            _connectionManager.SendMessage(new SelectCharacterRequest("BMoney!"));
        }

        public void HandleSelectCharacterResponse(RouteableMessage routeableMessage)
        {
            SelectCharacterResponse response = routeableMessage.Message as SelectCharacterResponse;

            if (response == null)
                return;

            //ummm do something i guess no idea what yet
        }

        public IEnumerator CheckForMessageUpdates()
        {
            ServerAnnouncement announcement = null;
            if (_serverWelcomeQueue.Count > 0)
                announcement = _serverWelcomeQueue.Dequeue();
            if (announcement == null)
            {
                yield return null;
            }
            else
            { _textLogDisplayManger.AddText(announcement.Message, announcement.AnnouncementType); }
        }

        public static AuthenticationAgent Instance()
        {
            if (!_authenticationAgent)
            {
                _authenticationAgent = FindObjectOfType(typeof(AuthenticationAgent)) as AuthenticationAgent;
            }
            return _authenticationAgent;
        }

    }
}
