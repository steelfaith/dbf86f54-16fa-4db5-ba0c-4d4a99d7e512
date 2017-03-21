using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;

namespace Assets.Scripts
{
    public class ButtonPowerDownScript : MonoBehaviour
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

        public void PowerDownAttack()
        {
            if (attackInfo.PowerLevel > 0)
                attackInfo.PowerLevel--;
        }

        private void UpdateButton()
        {
            button.interactable = attackInfo.IsCasting && attackInfo.CanPowerUp && attackInfo.PowerLevel > 0;
            button.image.sprite = button.interactable ? Resources.Load<Sprite>("power down") : Resources.Load<Sprite>("Blank");
        }
    }
}

