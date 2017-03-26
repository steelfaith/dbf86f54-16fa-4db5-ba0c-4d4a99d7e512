﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class ButtonPowerUpScript : MonoBehaviour
    {
        public Button button;
        public int attackIndex;
        private AttackInfo attackInfo;
        private FatbicController fatbic;
        private PlayerController player;
        private TextLogDisplayManager textLogDisplayManager;


        private void Awake()
        {
            fatbic = FatbicController.Instance();
            player = PlayerController.Instance();
        }

        private void Start()
        {
            
            textLogDisplayManager = TextLogDisplayManager.Instance();            
        }

        public void InitializeButton()
        {
            attackInfo = FatbicController.Instance().GetAttackInformation(attackIndex);
        }

        private void Update()
        {
            if(attackInfo != null)
                UpdateButton();
        }

        public void PowerUpAttack()
        {
            if(attackInfo.PowerLevel >= 3)
            {
                textLogDisplayManager.AddText("Attack can not be powered up farther!", AnnouncementType.System);
                return;
            }
            if(player.TryBurnPlayerResource(attackInfo.Affinity))
                attackInfo.PowerLevel++;
        }

        private void UpdateButton()
        {
            button.interactable = attackInfo.IsCasting && attackInfo.CanPowerUp;
            button.image.color = attackInfo.Affinity.GetColorFromMonsterAffinity();
            switch ((int)attackInfo.PowerLevel)
            {
                
                case 1:
                    button.image.sprite = Resources.Load<Sprite>("power up one");
                    break;
                case 2:
                    button.image.sprite = Resources.Load<Sprite>("power up two");
                    break;
                case 3:
                    button.image.sprite = Resources.Load<Sprite>("power up three");
                    break;

                default:
                    button.image.sprite = button.interactable? Resources.Load<Sprite>("power up one"): Resources.Load<Sprite>("Blank");
                    button.image.color = Color.white;
                    break;
            }
        }
    }
}
