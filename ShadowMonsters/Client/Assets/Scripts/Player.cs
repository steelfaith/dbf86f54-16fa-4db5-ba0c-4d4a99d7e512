using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class Player: MonoBehaviour
    {
        public Guid Id { get; set; }
        public List<GameObject> ControlledCreatures { get; set; }
        private MonsterSpawner _monsterSpawner;
        private PlayerData _currentData;

        private void Awake()
        {
            //this would probably be in some login object somewhere when we have that.  this would need to get that data.
            Id = ServerStub.Authenticate();
            _currentData = ServerStub.GetPlayerData(Id);
            ControlledCreatures = new List<GameObject>();        
        }

        private void Start()
        {
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



        private void AddControlledCreatures()
        {
            foreach (CreatureInfo item in _currentData.CurrentTeam)
            {
                var x = _monsterSpawner.SpawnMonster(item, true);
                x.gameObject.SetActive(false);
                ControlledCreatures.Add(x);
            }
        }
    }
}
