using System;
using Assets.Scripts.NetworkAgents;
using Client;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Responses;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButton : NetworkScriptBase<ConnectResponse>
{

    private readonly int _timeout = 30; // in seconds
    private bool _waitingOnLoginResponse;
    private DateTime? requestTime;
    private ClientConnectionManager _clientConnectionManager;
    private Button _button;

    private void Awake()
    {
        _clientConnectionManager = FindObjectOfType(typeof(ClientConnectionManager)) as ClientConnectionManager;
    }

    private void Start()
    {
        if (_clientConnectionManager != null)
            Start(_clientConnectionManager.Connection, OperationCode.ConnectResponse);
    }


    public void Update()
    {
        if (_waitingOnLoginResponse)
        {
            ConnectResponse response;
            if (TryGetResponse(out response))//try get response will lock the incoming response queue
            {
                _waitingOnLoginResponse = false;
                _clientConnectionManager.ClientId = response.ClientId;//this is also temporary, resolve in a better way after we have a user from the db
                SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
            }
            else
            {
                var now = DateTime.UtcNow;
                var elapsed = now.Subtract(requestTime.GetValueOrDefault());
                if (elapsed.Seconds > _timeout)
                {
                    _waitingOnLoginResponse = false;
                    _button.interactable = true;
                    requestTime = null;
                }
            }

        }
    }

    public void OnClicked(Button button)
    {
        if (_clientConnectionManager == null)
            return;

        if (button == null)
            return;

        _button = button;

        _button.interactable = false;

        requestTime = DateTime.UtcNow;
        _waitingOnLoginResponse = true;

        _clientConnectionManager.SendMessage(new ConnectRequest());
    }

    private void LoginResponseReceived(ConnectResponse response)
    {
        if (response == null)
            return;

        _waitingOnLoginResponse = false;
        
    }

}
