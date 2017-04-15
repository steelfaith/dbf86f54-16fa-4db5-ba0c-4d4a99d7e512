using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using System.Collections;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public Guid Id { get; set; }
        public GameObject statusDisplay;
        private PlayerData currentData;
        private ServerStub serverStub;
        private TextLogDisplayManager textLogDisplayManager;
        private StatusController statusController;
        private LightController lightController;
        public List<ElementalAffinity> currentResources;
        public bool CaughtBetweenPlanes { get; set; }


        public List<Guid> AttackIds { get; set; }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
            lightController = LightController.Instance();

            Id = serverStub.Authenticate();
            currentData = serverStub.GetPlayerData(Id);
            AttackIds = currentData.AttackIds;
            statusController = statusDisplay.GetComponentInChildren<StatusController>();
            statusController.displayName.color = Color.green;            
            SetPlayerData();
        }

        private void SetPlayerData()
        {
            statusController.SetMonster(currentData.PlayerDna.NickName,"0",MonsterPresence.Carnal,currentData.PlayerDna.CurrentHealth,currentData.PlayerDna.MaxHealth, currentData.Id);            
        }

        private void Update()
        {
            StartCoroutine(CheckForPlayerUpdates());
        }

        public IEnumerator CheckForPlayerUpdates()
        {
            var playerUpdate = serverStub.GetNextPlayerDataUpdate(Id);
            if (playerUpdate == null)
            {
                yield return null;
            }
            if(playerUpdate!=null)
                HandlePlayerUpdate(playerUpdate);
        }

        private void HandlePlayerUpdate(PlayerDataUpdate playerUpdate)
        {
            currentData = playerUpdate.Update;
            SetPlayerData();
            CaughtBetweenPlanes = currentData.PlayerDna.CurrentHealth < 1;
            if (CaughtBetweenPlanes)
            { lightController.ChangeToPlanesLight(); }
            else
            { lightController.ChangeToNormalLight(); }
            UpdateTeam();
        }

        private void UpdateTeam()
        {
            var teamController = TeamController.Instance();
            if (teamController == null) return;
            teamController.LoadTeam(currentData.CurrentTeam);
        }

        private static PlayerController playerController;       

        public static PlayerController Instance()
        {
            if (!playerController)
            {
                playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
                if (!playerController)
                    Debug.LogError("Could not find Player Status Controller");
            }
            return playerController;
        }

        public List<MonsterDna> GetTeam()
        {
            return currentData.CurrentTeam;
        }

        public PlayerData GetCurrentPlayerData()
        {
            return currentData;
        }

        public MonsterDna GetMonsterFromTeam(Guid id)
        {
            if (!currentData.CurrentTeam.Any()) return null;
            return currentData.CurrentTeam.FirstOrDefault(x => x.MonsterId == id);
        }

        public void PowerDownAttack(ElementalAffinity affinity)
        {
            //CollectResources(affinity);
            textLogDisplayManager.AddText(string.Format("You power down the attack and regain one {0} resource.", affinity.ToString()), AnnouncementType.System);
        }

        public void DisplayResources(ResourceUpdate resourceUpdate)
        {
            if (resourceUpdate == null) return;
            currentResources = resourceUpdate.Resources;
            statusController.UpdateResources(resourceUpdate.Resources);
        }

        internal void UpdateStatusController(AttackResolution attack)
        {
            statusController.UpdateMonster(attack);
        }

        internal void ClearResourceDisplay()
        {
            currentResources.Clear();
            statusController.UpdateResources(currentResources);
        }
    }
}
