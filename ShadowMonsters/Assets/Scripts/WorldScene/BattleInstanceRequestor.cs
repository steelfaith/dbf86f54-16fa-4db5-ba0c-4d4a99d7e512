using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.NetworkAgents;
using Client;
using Common.Messages;
using Common.Messages.Responses;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleInstanceRequestor : NetworkScriptBase<CreateBattleInstanceResponse>
{
    private ClientConnectionManager _clientConnectionManager;

    private void Awake()
    {
        _clientConnectionManager = FindObjectOfType(typeof(ClientConnectionManager)) as ClientConnectionManager;
    }

    private void Start()
    {
        if (_clientConnectionManager != null)
            Start(_clientConnectionManager.Connection, OperationCode.CreateBattleInstanceResponse);
    }

    private void Update()
    {
        //check the queue and switch to the battle instance with the proper data
        CreateBattleInstanceResponse response;
        if (TryGetResponse(out response))//try get response will lock the incoming response queue
        {
            SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Single);
        }
    }
}
