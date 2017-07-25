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
        private FatbicDisplayController fatbic;
        private PlayerController player;
        private TextLogDisplayManager textLogDisplayManager;
        private ButtonScript buttonScript;



        private void Awake()
        {
            fatbic = FatbicDisplayController.Instance();
            player = PlayerController.Instance();
        }

        private void Start()
        {
            
            textLogDisplayManager = TextLogDisplayManager.Instance();
            buttonScript = fatbic.GetButtonScriptFromIndex(attackIndex);
                      
        }

        public void InitializeButton()
        {
            attackInfo = FatbicDisplayController.Instance().GetAttackInformation(attackIndex);
        }

        private void Update()
        {
            if(attackInfo != null)
                UpdateButton();
        }

        public void PowerUpAttack()
        {
            fatbic.AttackPowerChange(attackInfo.AttackId, true);
        }

        private void UpdateButton()
        {
            button.interactable = buttonScript.IsCasting && attackInfo.CanPowerUp;
            button.image.color = attackInfo.Affinity.GetColorFromMonsterAffinity();
            switch (buttonScript.attackPower)
            {

                case PowerUpLevels.One:
                    button.image.sprite = Resources.Load<Sprite>("power up one");
                    break;
                case PowerUpLevels.Two:
                    button.image.sprite = Resources.Load<Sprite>("power up two");
                    break;
                case PowerUpLevels.Three:
                    button.image.sprite = Resources.Load<Sprite>("power up three");
                    break;

                default:
                    button.image.sprite = button.interactable ? Resources.Load<Sprite>("power up one") : Resources.Load<Sprite>("Blank");
                    button.image.color = Color.white;
                    break;
            }
        }
    }
}
