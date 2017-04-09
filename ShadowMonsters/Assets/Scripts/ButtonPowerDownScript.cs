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
        private FatbicDisplayController fatbic;
        private PlayerController player;


        private void Awake()
        {
            fatbic = FatbicDisplayController.Instance();
            player = PlayerController.Instance();
        }

        private void Start()
        {

        }
        public void InitializeButton()
        {
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
            //if (attackInfo == null) return;
            //button.interactable = attackInfo.IsCasting && attackInfo.CanPowerUp && attackInfo.PowerLevel > 0;
            //button.image.sprite = button.interactable ? Resources.Load<Sprite>("power down") : Resources.Load<Sprite>("Blank");
        }
    }
}

