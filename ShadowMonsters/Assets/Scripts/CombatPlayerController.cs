using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Collections;

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
        private FatbicDisplayController fatbic;
        private Guid attackInstanceId;
        private ServerStub serverStub;
        private TeamController teamController;
        private LightController lightController;

        public bool InCombat
        {
            get { return attackInstanceId != Guid.Empty; }
        }

        private bool IsIncarnated
        {
            get { return incarnatedMonster != null && incarnatedMonster.activeSelf; }
        }

        public Guid CurrentCombatantId {
            get
            {
                if (IsIncarnated)
                    return incarnatedMonster.GetComponent<BaseMonster>().MonsterId;
                else
                    return playerData.Id;
            }
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
            fatbic = FatbicDisplayController.Instance();
            teamController = TeamController.Instance();
            lightController = LightController.Instance();
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

        void Update()
        {
            
        }

        private static CombatPlayerController combatPlayerController;

        public static CombatPlayerController Instance()
        {
            if (!combatPlayerController)
            {
                combatPlayerController = FindObjectOfType(typeof(CombatPlayerController)) as CombatPlayerController;
            }
            return combatPlayerController;
        }

        public void StartCombat(Guid instanceId)
        {
            attackInstanceId = instanceId;

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
                serverStub.UpdateAttackInstance(new AttackUpdateRequest { AttackInstanceId = attackInstanceId, CurrentPlayerChampionId = playerChampionId });
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
                else
                {
                    DisplayPlayerDeath();
                }
                
            }
            
        }

        private void DisplayPlayerDeath()
        {
            DoAnimation(AnimationAction.Die);
            textLogDisplayManager.AddText(string.Format("You have lost the battle and are now caught between the plains! Find a plains walker to return you to your realm.", baseMonster.NickName), AnnouncementType.System);
            lightController.ChangeColor(new Color32(200, 9, 221, 255));
            playerController.CaughtBetweenPlains = true;
        }

        public void DoAnimation(AnimationAction action)
        {        
            animationController.PlayAnimation(IsIncarnated ? incarnatedMonster:gameObject, action);            
        }

        public void EndCombat()
        {
            attackInstanceId = Guid.Empty;
            RevertIncarnation();
            playerController.ClearResourceDisplay();
            DoAnimation(AnimationAction.Victory);
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
