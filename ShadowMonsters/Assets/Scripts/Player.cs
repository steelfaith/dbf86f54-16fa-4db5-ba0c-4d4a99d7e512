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
        public List<GameObject> ControlledCreatures { get; set; }        
        private MonsterSpawner _monsterSpawner;
        private PlayerData _currentData;
        private ServerStub serverStub;
        private TextLogDisplayManager textLogDisplayManager;
        public List<Guid> AttackIds { get; set; }
        private GameObject incarnatedMonster;
        private Animator anim;

        public Guid LeadMonsterId { get; set; }

        private void Awake()
        {
            //this would probably be in some login object somewhere when we have that.  this would need to get that data.
            Id = ServerStub.Authenticate();
            
            ControlledCreatures = new List<GameObject>();        
        }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
            anim = GetComponent<Animator>();
            _currentData = serverStub.GetPlayerData(Id);
            AttackIds = _currentData.AttackIds;
            _monsterSpawner = MonsterSpawner.Instance();            
            AddControlledCreatures();            
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
            var leadBaseCreature = lead.GetComponent<BaseCreature>();
            if (lead == null) return _player.Id;

            textLogDisplayManager.AddText(string.Format("You incarnate {0}.", leadBaseCreature.Name),AnnouncementType.Friendly);
            //omg blinky
            this.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            lead.gameObject.SetActive(true);

            incarnatedMonster = lead;
            return leadBaseCreature.MonsterId;
        }

        public void DoAttackAnimation()
        {
            anim.Play("", -1, 0f);
        }

        private GameObject GetLeadMonster()
        {
            foreach (GameObject go in ControlledCreatures)
            {
                var bc = go.GetComponent<BaseCreature>();
                if (bc.MonsterId == LeadMonsterId)
                    return go;
            }
            return null;
        }

        internal void RevertIncarnation()
        {
            textLogDisplayManager.AddText("You revert from your incarnated state.", AnnouncementType.Friendly);
            incarnatedMonster.SetActive(false);
            gameObject.SetActive(true);
        }

        private void AddControlledCreatures()
        {
            foreach (CreatureInfo item in _currentData.CurrentTeam)
            {
                if (item.IsTeamLead)
                    LeadMonsterId = item.MonsterId;
                var x = _monsterSpawner.SpawnMonster(item, true);
                x.gameObject.SetActive(false);
                ControlledCreatures.Add(x);
            }
        }
    }
}
