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

        private void Start()
        {
            playerController = PlayerController.Instance();
            monsterSpawner = MonsterSpawner.Instance();
            animationController = AnimationController.Instance();
            textLogDisplayManager = TextLogDisplayManager.Instance();
        }

        private void Update()
        {

        }

        private static CombatPlayerController _player;

        public static CombatPlayerController Instance()
        {
            if (!_player)
            {
                _player = FindObjectOfType(typeof(CombatPlayerController)) as CombatPlayerController;
                if (!_player)
                    Debug.LogError("Could not find Player!");
            }
            return _player;
        }

        internal Guid IncarnateMonster()
        {
            //perform some super sweet animation he e !!! wow!!  thrilling!! amazing!!!
            SpawnLeadMonster();
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

        internal void RevertIncarnation()
        {
            if (gameObject.activeSelf) return;
            textLogDisplayManager.AddText("You revert from your incarnated state.", AnnouncementType.Friendly);
            incarnatedMonster.SetActive(false);
            gameObject.SetActive(true);
        }

        private void SpawnLeadMonster()
        {
            var lead = playerController.GetLeadMonster();
            var spawned = monsterSpawner.SpawnMonster(lead, true);
            spawned.gameObject.SetActive(false);
            incarnatedMonster = spawned.gameObject;
        }
    }
}
