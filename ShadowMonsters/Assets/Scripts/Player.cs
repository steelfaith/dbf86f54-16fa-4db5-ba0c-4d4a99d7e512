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
    public class Player: MonoBehaviour
    {
        public Guid Id { get; set; }
        public List<GameObject> ControlledMonsters { get; set; }
        public GameObject statusDisplay;
        private MonsterSpawner monsterSpawner;
        private PlayerData currentData;
        private ServerStub serverStub;
        private TextLogDisplayManager textLogDisplayManager;
        public List<Guid> AttackIds { get; set; }
        private GameObject incarnatedMonster;
        private StatusController statusController;
        private BaseMonster baseMonster; //we are all monsters inside....
        private AnimationController animationController;

        private void Awake()
        {          
            ControlledMonsters = new List<GameObject>();        
        }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            monsterSpawner = MonsterSpawner.Instance();
            animationController = AnimationController.Instance();
            baseMonster = GetComponent<BaseMonster>();
            //this would probably be in some login object somewhere when we have that.  this would need to get that data.
            Id = serverStub.Authenticate();
            currentData = serverStub.GetPlayerData(Id);
            baseMonster.DisplayName = currentData.DisplayName;
            baseMonster.CurrentHealth = currentData.CurrentHealth;
            baseMonster.MaxHealth = currentData.MaximumHealth;
            baseMonster.MonsterAffinity = ElementalAffinity.Human;
            baseMonster.NameKey = "unitychan";
            textLogDisplayManager = TextLogDisplayManager.Instance();
            
            
            AttackIds = currentData.AttackIds;
        
            AddControlledMonsters();
            statusController = statusDisplay.GetComponentInChildren<StatusController>();
            statusController.displayName.color = Color.green;
            statusController.SetMonster(baseMonster);      
        }

        internal void CollectResources(int cooldown, ElementalAffinity affinity)
        {
            //move to server
            var result = UnityEngine.Random.Range(1, 101);
            if(result < cooldown *20)
            {
                statusController.AddResource(affinity);
            }
        }

        private void Update()
        {

        }

        private static Player _player;

        public static Player Instance()
        {
            if (!_player)
            {
                _player = FindObjectOfType(typeof(Player)) as Player;
                if (!_player)
                    Debug.LogError("Could not find Player!");
            }
            return _player;
        }

        internal Guid IncarnateMonster()
        {
            //perform some super sweet animation here !!! wow!!  thrilling!! amazing!!!
            GameObject lead = GetLeadMonster();           
            if (lead == null) return _player.Id;
            var leadBaseMonster = lead.GetComponent<BaseMonster>();

            textLogDisplayManager.AddText(string.Format("You incarnate {0}.", leadBaseMonster.DisplayName),AnnouncementType.Friendly);
            //omg blinky
            gameObject.SetActive(true);
            gameObject.SetActive(false);
            lead.gameObject.SetActive(true);

            incarnatedMonster = lead;
            return leadBaseMonster.MonsterId;
        }

        public void DoAnimation(AnimationAction action)
        {
            GameObject lead = GetLeadMonster();
            var leadIsActive = lead != null && lead.activeSelf;
            {
                animationController.PlayAnimation(leadIsActive?lead:gameObject, action);
            }
        }

        public GameObject GetLeadMonster()
        {
            foreach (GameObject go in ControlledMonsters)
            {
                var bc = go.GetComponent<BaseMonster>();
                if (bc.IsPlayerTeamLead && bc.CurrentHealth > 0)
                { return go; }
            }
            var newLead = ControlledMonsters.FirstOrDefault(monster=>monster.GetComponent<BaseMonster>().CurrentHealth > 0);
            UpdateLead(newLead);
            return newLead;
        }

        private void UpdateLead(GameObject newLead)
        {
            foreach (GameObject go in ControlledMonsters)
            {
                var lead = newLead.GetComponent<BaseMonster>();
                var bm = go.GetComponent<BaseMonster>();
                bm.IsPlayerTeamLead = (bm.MonsterId == lead.MonsterId);
            }
        }

        internal void RevertIncarnation()
        {
            if (gameObject.activeSelf) return;
            textLogDisplayManager.AddText("You revert from your incarnated state.", AnnouncementType.Friendly);
            incarnatedMonster.SetActive(false);
            gameObject.SetActive(true);
        }

        private void AddControlledMonsters()
        {
            foreach (MonsterInfo item in currentData.CurrentTeam)
            {
                var x = monsterSpawner.SpawnMonster(item, true);
                x.gameObject.SetActive(false);
                ControlledMonsters.Add(x);
            }
        }
    }
}
