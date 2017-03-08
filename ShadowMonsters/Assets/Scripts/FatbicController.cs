using Assets.Infrastructure;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FatbicController : MonoBehaviour
    {
        public GameObject _modalPanel;

        public Button attackOneButton;        
        public Button attackTwoButton;
        public Button attackThreeButton;
        public Button attackFourButton;      
        public Button attackFiveButton;
        public Button attackSixButton;


        private void Awake()
        {

        }

        private void Start()
        {

        }


        private static FatbicController fatbicController;

        public static FatbicController Instance()
        {
            if (!fatbicController)
            {
                fatbicController = FindObjectOfType(typeof(FatbicController)) as FatbicController;
                if (!fatbicController)
                    Debug.LogError("Could not find FATBITCH!");
            }
            return fatbicController;
        }

        public void BeginAttack(UnityAction attackOne, UnityAction attackTwo, UnityAction attackThree, UnityAction attackFour, UnityAction attackFive, UnityAction attackSix)
        {
            _modalPanel.SetActive(true);

            attackOneButton.onClick.RemoveAllListeners();
            attackOneButton.onClick.AddListener(attackOne);

            attackTwoButton.onClick.RemoveAllListeners();
            attackTwoButton.onClick.AddListener(attackTwo);

            attackThreeButton.onClick.RemoveAllListeners();
            attackThreeButton.onClick.AddListener(attackThree);

            attackFourButton.onClick.RemoveAllListeners();
            attackFourButton.onClick.AddListener(attackFour);

            attackFiveButton.onClick.RemoveAllListeners();
            attackFiveButton.onClick.AddListener(attackFive);

            attackSixButton.onClick.RemoveAllListeners();
            attackSixButton.onClick.AddListener(attackSix);

        }


        public void LoadAttacks(List<AttackInfo> orderedAttackInfo)
        {
            if(orderedAttackInfo.Count == 0 || orderedAttackInfo.Count > 5)
            {
                Debug.LogError("Attack count outside valid value of 1 to 6");
                return;
            }


        }
    }
}
