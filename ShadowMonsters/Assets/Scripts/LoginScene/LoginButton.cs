using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NetworkAgents;
using Client;
using Common.Messages;
using Common.Messages.Responses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour {

    private readonly int _timeout = 30; // in seconds
    private bool _waitingOnLoginResponse;
    private DateTime? requestTime;
    private AuthenticationAgent _authenticationAgent;
    private Button _button;

    private void Start()
    {
        _authenticationAgent = FindObjectOfType(typeof(AuthenticationAgent)) as AuthenticationAgent;
        _authenticationAgent.RegisterForResponse("LoginScript", new NetworkResponseAction(LoginSuccessful, typeof(ConnectResponse)));
    }

    public void Update()
    {
        if (_waitingOnLoginResponse)
        {
            var now = DateTime.UtcNow;
            var elapsed = now.Subtract(requestTime.GetValueOrDefault());
            if (elapsed.Seconds > _timeout)
            {
                _waitingOnLoginResponse = false;
                _button.interactable = true;
                requestTime = null;
            }

            //StartCoroutine(CheckAuthenticationAgentForSuccess());
        }
    }

    public void OnClicked(Button button)
    {
        if (_authenticationAgent == null)
            return;

        if (button == null)
            return;

        _button = button;

        _button.interactable = false;

        requestTime = DateTime.UtcNow;
        _waitingOnLoginResponse = true;

        _authenticationAgent.Login();
    }

    public IEnumerator CheckAuthenticationAgentForSuccess()
    {
        if (_authenticationAgent == null)
            yield return null;

        if (_authenticationAgent.LoginSuccessful)
        {
            _waitingOnLoginResponse = false;
            SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
        }

    }

    private void LoginSuccessful(Message message)
    {
        ConnectResponse response = message as ConnectResponse;
        if (response == null)
            return;

        _waitingOnLoginResponse = false;
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

}
