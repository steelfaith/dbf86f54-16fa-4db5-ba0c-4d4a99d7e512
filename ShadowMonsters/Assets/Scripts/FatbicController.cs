using Assets.Infrastructure;
using Assets.ServerStubHome;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

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
        public Button pUpOneButton;
        public Button pUpTwoButton;
        public Button pUpThreeButton;
        public Button pUpFourButton;
        public Button pUpFiveButton;
        public Button bondButton;
        public Button runButton;
        public Button pDownOneButton;
        public Button pDownTwoButton;
        public Button pDownThreeButton;
        public Button pDownFourButton;
        public Button pDownFiveButton;
        public List<ButtonScript> attackButtonScripts = new List<ButtonScript>();
        public event EventHandler<DataEventArgs<AttackInfo>> AttackAttempt;
        private List<AttackInfo> attackInfoList;
        private ServerStub serverStub;
        private PlayerController player;

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
            serverStub = ServerStub.Instance();
            player = PlayerController.Instance();
            LoadAttacks();
            InitializePowerUpPress(OnPowerUpOnePressed, OnPowerUpTwoPressed, OnPowerUpThreePressed, OnPowerUpFourPressed, OnPowerUpFivePressed);
            InitializePowerDownPress(OnPowerDownOnePressed, OnPowerDownTwoPressed, OnPowerDownThreePressed, OnPowerDownFourPressed, OnPowerDownFivePressed);
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

        private void InitializePowerUpPress(UnityAction pUpOne, UnityAction pUpTwo, UnityAction pUpThree, UnityAction pUpFour, UnityAction pUpFive)
        {
             
            pUpOneButton.onClick.RemoveAllListeners();
            pUpOneButton.onClick.AddListener(pUpOne);

            pUpTwoButton.onClick.RemoveAllListeners();
            pUpTwoButton.onClick.AddListener(pUpTwo);

            pUpThreeButton.onClick.RemoveAllListeners();
            pUpThreeButton.onClick.AddListener(pUpThree);

            pUpFourButton.onClick.RemoveAllListeners();
            pUpFourButton.onClick.AddListener(pUpFour);

            pUpFiveButton.onClick.RemoveAllListeners();
            pUpFiveButton.onClick.AddListener(pUpFive);

        }

        private void InitializePowerDownPress(UnityAction pDownOne, UnityAction pDownTwo, UnityAction pDownThree, UnityAction pDownFour, UnityAction pDownFive)
        {

            pDownOneButton.onClick.RemoveAllListeners();
            pDownOneButton.onClick.AddListener(pDownOne);

            pDownTwoButton.onClick.RemoveAllListeners();
            pDownTwoButton.onClick.AddListener(pDownTwo);

            pDownThreeButton.onClick.RemoveAllListeners();
            pDownThreeButton.onClick.AddListener(pDownThree);

            pDownFourButton.onClick.RemoveAllListeners();
            pDownFourButton.onClick.AddListener(pDownFour);

            pDownFiveButton.onClick.RemoveAllListeners();
            pDownFiveButton.onClick.AddListener(pDownFive);

        }

        private void OnPowerUpOnePressed()
        {
            pUpOneButton.GetComponent<ButtonPowerUpScript>().PowerUpAttack();
        }

        private void OnPowerUpTwoPressed()
        {
            pUpTwoButton.GetComponent<ButtonPowerUpScript>().PowerUpAttack();
        }

        private void OnPowerUpThreePressed()
        {
            pUpThreeButton.GetComponent<ButtonPowerUpScript>().PowerUpAttack();
        }

        private void OnPowerUpFourPressed()
        {
            pUpFourButton.GetComponent<ButtonPowerUpScript>().PowerUpAttack();
        }

        private void OnPowerUpFivePressed()
        {
            pUpFiveButton.GetComponent<ButtonPowerUpScript>().PowerUpAttack();
        }

        private void OnPowerDownOnePressed()
        {
            pDownOneButton.GetComponent<ButtonPowerDownScript>().PowerDownAttack();
        }

        private void OnPowerDownTwoPressed()
        {
            pDownTwoButton.GetComponent<ButtonPowerDownScript>().PowerDownAttack();
        }

        private void OnPowerDownThreePressed()
        {
            pDownThreeButton.GetComponent<ButtonPowerDownScript>().PowerDownAttack();
        }

        private void OnPowerDownFourPressed()
        {          
            pDownFourButton.GetComponent<ButtonPowerDownScript>().PowerDownAttack();
        }

        private void OnPowerDownFivePressed()
        {
            pDownFiveButton.GetComponent<ButtonPowerDownScript>().PowerDownAttack();
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
            var teamLeadId = player.GetLeadMonster().MonsterId;
            var alive = serverStub.CheckPulse(teamLeadId);

            var attacks = alive ? serverStub.GetAttacksForMonster(teamLeadId) : serverStub.GetAttacksForPlayer(player.Id);
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
