using System;
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


        private void Awake()
        {

        }

        private void Start()
        {
            fatbic = FatbicController.Instance();
            attackInfo = fatbic.GetAttackInformation(attackIndex);
        }

        private void Update()
        {
            UpdateButton();
        }

        public void PowerUpAttack()
        {
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
