using Assets.Infrastructure;
using System;
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
        public Button stopAttackButton;
        public Button bondButton;
        public Button runButton;
        public List<ButtonScript> attackButtonScripts = new List<ButtonScript>();
        public event EventHandler<DataEventArgs<AttackInfo>> AttackAttempt;
        private List<AttackInfo> attackInfoList;

        public AttackInfo GetAttackInformation(int attackIndex)
        {
            if(attackInfoList == null)
            {
                Debug.LogError("Need to get attacks from server");
            }
            return attackInfoList[attackIndex];
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

        public void BeginAttack(UnityAction attackOne, UnityAction attackTwo, UnityAction attackThree, UnityAction attackFour, UnityAction attackFive, UnityAction stopAttack,
                                    UnityAction bondEssence, UnityAction runAway)
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

            stopAttackButton.onClick.RemoveAllListeners();
            stopAttackButton.onClick.AddListener(stopAttack);

            bondButton.onClick.RemoveAllListeners();
            bondButton.onClick.AddListener(bondEssence);

            runButton.onClick.RemoveAllListeners();
            runButton.onClick.AddListener(runAway);
        }


        public void StartGlobalRecharge(int recharge, int exclusionIndex)
        {
            foreach (ButtonScript item in attackButtonScripts)
            {
                if(item.attackIndex != exclusionIndex)
                //TODO: global cooldown should be reduced by speed /1000 ***monster speed should absolutely cap at 499
                item.StartGlobalCooldown(recharge);
            }
        }

        public void LoadAttacks() //prob need to pass monster id
        {
            var attacks = ServerStub.GetAttackInfo(Guid.NewGuid());
            if (attacks.Count == 0 || attacks.Count > 5)
            {
                Debug.LogError("Attack count outside valid value of 1 to 5");
                return;
            }
            attackInfoList = attacks;
        }

        private void OnDestroy()
        {
            foreach (var item in attackButtonScripts)
            {
                item.AttackAttempt -= AttackAttemptFromButton;
            }
        }
        private void Awake()
        {
            attackButtonScripts.Add(attackOneButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackTwoButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackThreeButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackFourButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackFiveButton.GetComponent<ButtonScript>());
            LoadAttacks();
            AttachButtonScriptEvents();
        }

        private void AttachButtonScriptEvents()
        {
            foreach (var item in attackButtonScripts)
            {
                item.AttackAttempt += AttackAttemptFromButton;
            }
        }

        private void AttackAttemptFromButton(object sender, DataEventArgs<AttackInfo> e)
        {
            var handler = AttackAttempt;
            if (handler != null) AttackAttempt(this, e);
        }
    }
}
