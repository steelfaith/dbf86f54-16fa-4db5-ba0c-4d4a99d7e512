using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
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
        private Guid combatInstanceId;
        private ServerStub serverStub;
        private TeamController teamController;

        public bool InCombat
        {
            get { return combatInstanceId != Guid.Empty; }
        }

        private bool IsIncarnated
        {
            get { return incarnatedMonster != null && incarnatedMonster.activeSelf; }
        }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            playerController = PlayerController.Instance();
            baseMonster = GetComponent<BaseMonster>();
            monsterSpawner = MonsterSpawner.Instance();
            animationController = AnimationController.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
            incarnationContainer = IncarnationContainer.Instance();
            fatbic = FatbicController.Instance();
            teamController = TeamController.Instance();
            playerData = playerController.GetCurrentPlayerData();
            SetupBaseMonsterForPlayer();
            
        }

        private void SetupBaseMonsterForPlayer()
        {
            baseMonster.NameKey = "unitychan";
            baseMonster.NickName = playerData.PlayerDna.NickName;
            baseMonster.CurrentHealth = playerData.PlayerDna.CurrentHealth;
            baseMonster.MaxHealth = playerData.PlayerDna.MaxHealth;
            baseMonster.MonsterId = playerData.Id;
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

        public void StartCombat(Guid instanceId)
        {
            combatInstanceId = instanceId;

            IncarnateMonster();
        }

        internal void IncarnateMonster()
        {
            //perform some super sweet animation he e !!! wow!!  thrilling!! amazing!!!
            Guid playerChampionId = Guid.Empty;

            if(incarnatedMonster != null)
            {
                var baseMonster = incarnatedMonster.GetComponent<BaseMonster>();
                if(baseMonster.MonsterId == incarnationContainer.MonsterId)
                {
                    //no need to incarnate
                    playerChampionId = baseMonster.MonsterId;
                }
                Destroy(baseMonster.gameObject);
            }

            
            incarnatedMonster = SpawnLeadMonster();
            fatbic.LoadAttacks();
            fatbic.StartGlobalRecharge(2, null);
               

            if (incarnatedMonster != null)
            {
                var incarnatedBaseMonster = incarnatedMonster.GetComponent<BaseMonster>();

                textLogDisplayManager.AddText(string.Format("You incarnate {0}.", incarnatedBaseMonster.DisplayName), AnnouncementType.Friendly);
                //omg blinky
                gameObject.SetActive(true);
                gameObject.SetActive(false);
                incarnatedMonster.gameObject.SetActive(true);
                playerChampionId = incarnatedBaseMonster.MonsterId;
            }
            else
            {
                playerChampionId = playerController.Id;
            }

            if(InCombat)
            {
                serverStub.UpdateAttackInstance(new AttackUpdateRequest { AttackInstanceId = combatInstanceId, CurrentPlayerChampionId = playerChampionId });
            }
        }

        public void UpdateTeamHealth(AttackResolution attackResult)
        {
            DoAnimation(AnimationAction.GetHit);
            teamController.UpdateLead(attackResult);
            
            if(attackResult.WasFatal)
            {
                if (IsIncarnated)
                {
                    //current incarnated monster has died...
                    DoAnimation(AnimationAction.Die);
                    var baseMonster = incarnatedMonster.GetComponent<BaseMonster>();
                    textLogDisplayManager.AddText(string.Format("{0} has died!", baseMonster.NickName), AnnouncementType.System);
                    RevertIncarnation();
                    teamController.HandleCodeDrop(incarnationContainer.item);
                    DoAnimation(AnimationAction.Knockback);
                }
                
            }
            
        }

        private void IncarnateNextLivingEssence()
        {
            textLogDisplayManager.AddText("You quickly incarnate the next member of your team!", AnnouncementType.System);
        }

        public void DoAnimation(AnimationAction action)
        {        
            animationController.PlayAnimation(IsIncarnated ? incarnatedMonster:gameObject, action);            
        }

        public void EndCombat()
        {
            combatInstanceId = Guid.Empty;
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
            incarnatedMonster.SetActive(false);
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
