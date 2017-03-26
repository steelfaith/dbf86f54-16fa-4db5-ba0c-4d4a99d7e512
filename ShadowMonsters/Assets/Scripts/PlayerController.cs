using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
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
        public List<ElementalAffinity> currentResources;


        public List<Guid> AttackIds { get; set; }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();

            Id = serverStub.Authenticate();
            currentData = serverStub.GetPlayerData(Id);
            AttackIds = currentData.AttackIds;
            statusController = statusDisplay.GetComponentInChildren<StatusController>();
            statusController.displayName.color = Color.green;
            statusController.SetMonster(currentData.DisplayName,"0",MonsterPresence.Carnal,currentData.CurrentHealth,currentData.MaximumHealth, currentData.Id);
            
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
        internal void CollectResources(ElementalAffinity affinity)
        {

            var response = serverStub.AddPlayerResource(new AddResourceRequest
            {
                PlayerId = Id,
                Affinity = affinity,
            });

            currentResources = response.Resources;

            statusController.UpdateResources(response.Resources);
        }
        public void PowerDownAttack(ElementalAffinity affinity)
        {
            CollectResources(affinity);
            textLogDisplayManager.AddText(string.Format("You power down the attack and regain one {0} resource.", affinity.ToString()), AnnouncementType.System);
        }

        public bool TryBurnPlayerResource(ElementalAffinity resource)
        {
            var response = serverStub.BurnResource(new BurnResourceRequest { NeededResource = resource, PlayerId = Id });
            if (response == null) return false;
            if (response.PlayerId != Id) return false;
            currentResources = response.CurrentResources;
            statusController.UpdateResources(response.CurrentResources);
            textLogDisplayManager.AddText(response.Success ? "You powered up the attack!" : string.Format("Not enough {0} resources to power up this attack.", resource.ToString()), AnnouncementType.System);
            return response.Success;
        }

        internal void ClearResources()
        {
            currentResources = serverStub.ClearPlayerResources(Id);
            statusController.UpdateResources(currentResources);
        }
    }
}
