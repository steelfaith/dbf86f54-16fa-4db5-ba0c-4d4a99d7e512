using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;

using System.Threading;
using UnityEngine.SceneManagement;



namespace Assets.Scripts
{
    public class CombatPlayerController: MonoBehaviour
    {
        private MonsterSpawner monsterSpawner;
        private GameObject incarnatedMonster;
        private BaseMonster baseMonster; //we are all monsters inside....
        private AnimationController animationController;
        private PlayerController playerController;
        private TextLogDisplayManager textLogDisplayManager;
        private PlayerData playerData;
        private IncarnationContainer incarnationContainer;
        private FatbicController fatbic;
        public bool InCombat { get; set; }

        private void Start()
        {
            playerController = PlayerController.Instance();
            baseMonster = GetComponent<BaseMonster>();
            monsterSpawner = MonsterSpawner.Instance();
            animationController = AnimationController.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
            incarnationContainer = IncarnationContainer.Instance();
            fatbic = FatbicController.Instance();
            playerData = playerController.GetCurrentPlayerData();
            SetupBaseMonsterForPlayer();
            
        }

        private void SetupBaseMonsterForPlayer()
        {
            baseMonster.NameKey = "unitychan";
            baseMonster.NickName = playerData.DisplayName;
            baseMonster.CurrentHealth = playerData.CurrentHealth;
            baseMonster.MaxHealth = playerData.MaximumHealth;
        }

        private void Update()
        {

        }

        private static CombatPlayerController combatPlayerController;

        public static CombatPlayerController Instance()
        {
            if (!combatPlayerController)
            {
                combatPlayerController = FindObjectOfType(typeof(CombatPlayerController)) as CombatPlayerController;
                if (!combatPlayerController)
                    Debug.LogError("Could not find Player!");
            }
            return combatPlayerController;
        }

        public Guid StartCombat()
        {
            InCombat = true;

            return IncarnateMonster();
        }

        internal Guid IncarnateMonster()
        {
            //perform some super sweet animation he e !!! wow!!  thrilling!! amazing!!!

            if(incarnatedMonster != null)
            {
                var baseMonster = incarnatedMonster.GetComponent<BaseMonster>();
                if(baseMonster.MonsterId == incarnationContainer.MonsterId)
                {
                    //no need to incarnate
                    return baseMonster.MonsterId;
                }
                Destroy(baseMonster.gameObject);
            }

            
            incarnatedMonster = SpawnLeadMonster();
            fatbic.LoadAttacks();
            fatbic.StartGlobalRecharge(2, null);
            if (incarnatedMonster == null) return playerController.Id;
            var incarnatedBaseMonster = incarnatedMonster.GetComponent<BaseMonster>();

            textLogDisplayManager.AddText(string.Format("You incarnate {0}.", incarnatedBaseMonster.DisplayName),AnnouncementType.Friendly);
            //omg blinky
            gameObject.SetActive(true);
            gameObject.SetActive(false);
            incarnatedMonster.gameObject.SetActive(true);
            
            return incarnatedBaseMonster.MonsterId;
        }

        public void DoAnimation(AnimationAction action)
        {
            var leadIsActive = incarnatedMonster != null && incarnatedMonster.activeSelf;            
            animationController.PlayAnimation(leadIsActive?incarnatedMonster:gameObject, action);            
        }

        public void EndCombat()
        {
            InCombat = false;
            RevertIncarnation();
            playerController.ClearResources();
        }

        internal void RevertIncarnation()
        {
            if (gameObject.activeSelf) return;
            textLogDisplayManager.AddText("You revert from your incarnated state.", AnnouncementType.Friendly);
            gameObject.SetActive(true);
            if(InCombat)
                fatbic.LoadAttacks();
            Destroy(incarnatedMonster);
        }

        private GameObject SpawnLeadMonster()
        {         
            
            var lead = playerController.GetMonsterFromTeam(incarnationContainer.MonsterId);
            
            if (lead == null) return null;
            var spawned = monsterSpawner.SpawnMonster(lead, true);
            spawned.gameObject.SetActive(false);
            return spawned.gameObject;
        }
    }
}
