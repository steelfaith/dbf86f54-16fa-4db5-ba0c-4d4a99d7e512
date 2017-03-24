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
        private PlayerController player;


        private void Awake()
        {

        }

        private void Start()
        {
            fatbic = FatbicController.Instance();
            player = PlayerController.Instance();
            attackInfo = fatbic.GetAttackInformation(attackIndex);
        }

        private void Update()
        {
            UpdateButton();
        }

        public void PowerDownAttack()
        {
            if (attackInfo.PowerLevel > 0)
            {
                player.PowerDownAttack(attackInfo.Affinity);
                attackInfo.PowerLevel--;
            }
                
        }

        private void UpdateButton()
        {
            button.interactable = attackInfo.IsCasting && attackInfo.CanPowerUp && attackInfo.PowerLevel > 0;
            button.image.sprite = button.interactable ? Resources.Load<Sprite>("power down") : Resources.Load<Sprite>("Blank");
        }
    }
}

