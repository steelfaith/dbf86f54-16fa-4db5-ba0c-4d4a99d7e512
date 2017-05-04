
using Assets.ServerStubHome.MessageHandlers;
using UnityEngine;
using Assets.Scripts;
using Common.Messages;
using System.Collections;
using System.Collections.Generic;
using Common.Messages.Requests;

namespace Assets.ServerStubHome
{
    public class AuthenticationAgent : MonoBehaviour
    {

        public ConnectionResponseHandler ConnectionResponse { get; set; }
        private TextLogDisplayManager _textLogDisplayManger;
        private static AuthenticationAgent _authenticationAgent;
        private Queue<ServerAnnouncement> _serverWelcomeQueue = new Queue<ServerAnnouncement>();
        private ClientConnectionManager _connectionManager;

        private void Awake()
        {
             ConnectionResponse = new ConnectionResponseHandler(this);
            _connectionManager = GetComponentInParent<ClientConnectionManager>();
        }

        private void Start()
        {                       
            _textLogDisplayManger = TextLogDisplayManager.Instance();
            //TODO: Call this here until we get a login screen
            Login();
        }

        public void Login()
        {
            _connectionManager.SendMessage(new ConnectRequest(1));
        }

        private void Update()
        {
            StartCoroutine(CheckForMessageUpdates());
        }

        public void HandleAnnouncement(ServerAnnouncement announcement)
        {
            _serverWelcomeQueue.Enqueue(announcement);  
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
