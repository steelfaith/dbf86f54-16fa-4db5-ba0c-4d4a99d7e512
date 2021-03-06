﻿using Assets.Infrastructure;
using Assets.ServerStubHome;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

namespace Assets.Scripts
{
    public class FatbicDisplayController : MonoBehaviour
    {
        public GameObject fatbicPanel;

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
        public List<ButtonPowerUpScript> powerUpButtonScripts = new List<ButtonPowerUpScript>();
        public List<ButtonPowerDownScript> powerDownButtonScripts = new List<ButtonPowerDownScript>();
        public bool IsBusy;
        private List<AttackInfo> attackInfoList;
        private ServerStub serverStub;
        private PlayerController player;
        private IncarnationContainer incarnationContainer;
        private Dictionary<Guid, ButtonScript> attackIdToButtonMapping = new Dictionary<Guid, ButtonScript>();

        public Guid AttackInstanceId { get; set; }

        public AttackInfo GetAttackInformation(int attackIndex)
        {
            if(attackInfoList == null)
            {
                Debug.LogError("Need to get attacks from server");
            }
            return attackInfoList[attackIndex];
        }

        public AttackInfo RegisterAttackButton(ButtonScript buttonScript, int index)
        {
            var attackInfo = GetAttackInformation(index);
            attackIdToButtonMapping[attackInfo.AttackId]= buttonScript;
            return attackInfo;
        }

        private void Start()
        {
            serverStub = ServerStub.Instance();
            player = PlayerController.Instance();
            incarnationContainer = IncarnationContainer.Instance();
            InitializePowerUpPress(OnPowerUpOnePressed, OnPowerUpTwoPressed, OnPowerUpThreePressed, OnPowerUpFourPressed, OnPowerUpFivePressed);
            InitializePowerDownPress(OnPowerDownOnePressed, OnPowerDownTwoPressed, OnPowerDownThreePressed, OnPowerDownFourPressed, OnPowerDownFivePressed);
        }

        private void EndCombat()
        {
            //TODO this type of clearing is probably not necessary as the combat scene is unloaded when combat is over, but check
            attackIdToButtonMapping.Clear();
        }
        void Update()
        {
            StartCoroutine(CheckForButtonUpdates());
            StartCoroutine(CheckForResourceGain());
            StartCoroutine(CheckForAttackPowerUpdates());
        }
        public IEnumerator CheckForResourceGain()
        {
            var resourceUpdate = serverStub.GetNextAddResourceUpdate(AttackInstanceId);
            if (resourceUpdate == null)
            {
                yield return null;
            }
            HandleResourceUpdates(resourceUpdate);
        }

        private void HandleResourceUpdates(ResourceUpdate resourceUpdate)
        {
            player.DisplayResources(resourceUpdate);
        }

        public IEnumerator CheckForButtonUpdates()
        {
            var buttonUpdate = serverStub.GetNextButtonUpdate(AttackInstanceId);
            if (buttonUpdate == null)
            {
                yield return null;
            }
            HandleButtonUpdates(buttonUpdate);
        }

        public IEnumerator CheckForAttackPowerUpdates()
        {
            var attackPowerUpdate = serverStub.GetNextAttackPowerUpdate(AttackInstanceId);
            if (attackPowerUpdate == null)
            {
                yield return null;
            }
            HandleAttackPowerUpdates(attackPowerUpdate);
        }

        public void AttackPowerChange(Guid attackId, bool up)
        {
            serverStub.ChangeAttackPower(new AttackPowerChangeRequest
            {
                AttackId = attackId,
                AttackInstanceId = AttackInstanceId,
                Up = up,
            });
        }

        internal void Reset()
        {
            foreach(ButtonScript bs in attackButtonScripts)
            {
                bs.Reset();
            }
        }

        private void HandleAttackPowerUpdates(AttackPowerChangeResolution result)
        {
            if (result == null) return;
            ButtonScript button;
            attackIdToButtonMapping.TryGetValue(result.AttackId, out button);
            if (button == null) return;
            button.attackPower = (PowerUpLevels)Enum.Parse(typeof(PowerUpLevels), result.PowerLevel.ToString());
        }

        private void HandleButtonUpdates(ButtonPressResolution buttonUpdate)
        {
            if (buttonUpdate == null) return;
            if(buttonUpdate.AttackId == null || buttonUpdate.AttackId == Guid.Empty)
            {
                StartGlobalRecharge(buttonUpdate.TimeOutSeconds, null);
                return;
            }

            ButtonScript button;
            attackIdToButtonMapping.TryGetValue(buttonUpdate.AttackId, out button);
            if (button == null) return;


            button.StartButtonAction(buttonUpdate.TimeOutSeconds);
            StartGlobalRecharge(buttonUpdate.TimeOutSeconds,button.attackIndex);
        }

        private static FatbicDisplayController fatbicController;

        public static FatbicDisplayController Instance()
        {
            if (!fatbicController)
            {
                fatbicController = FindObjectOfType(typeof(FatbicDisplayController)) as FatbicDisplayController;
            }
            return fatbicController;
        }

        public void BeginAttack(UnityAction attackOne, UnityAction attackTwo, UnityAction attackThree, UnityAction attackFour, UnityAction attackFive, UnityAction stopAttack,
                                    UnityAction bondEssence, UnityAction runAway)
        {
            fatbicPanel.SetActive(true);

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

        public void StartGlobalRecharge(float recharge, int? exclusionIndex)
        {
            foreach (ButtonScript item in attackButtonScripts)
            {

                if (exclusionIndex == null)
                {
                    item.StartGlobalCooldown(recharge);
                    continue;
                }

                //TODO: global cooldown should be reduced by speed /1000 ***monster speed should absolutely cap at 499
                if (item.attackIndex != exclusionIndex.Value)
                {
                    item.StartGlobalCooldown(recharge);
                }

            }
        }

        public void LoadAttacks() 
        {
            List<AttackInfo> attacks = null;
            if (serverStub.CheckPulse(incarnationContainer.MonsterId))
            { attacks = serverStub.GetAttacksForMonster(incarnationContainer.MonsterId); }
            else
            { attacks = serverStub.GetAttacksForPlayer(player.Id); }

            if (attacks == null || attacks.Count == 0 || attacks.Count > 5)
            {
                Debug.LogError("Attack count outside valid value of 1 to 5");
                return;
            }
            attackInfoList = attacks;
            InitializeButtons();
        }

        public ButtonScript GetButtonScriptFromIndex(int index)
        {
            return attackButtonScripts[index];
        }

        private void InitializeButtons()
        {
            foreach (var item in attackButtonScripts)
            {
                item.InitializeButton();
            }

            foreach (var item in powerUpButtonScripts)
            {
                item.InitializeButton();
            }

            foreach (var item in powerDownButtonScripts)
            {
                item.InitializeButton();
            }
        }

        private void Awake()
        {
            attackButtonScripts.Add(attackOneButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackTwoButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackThreeButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackFourButton.GetComponent<ButtonScript>());
            attackButtonScripts.Add(attackFiveButton.GetComponent<ButtonScript>());

            powerDownButtonScripts.Add(pDownOneButton.GetComponent<ButtonPowerDownScript>());
            powerDownButtonScripts.Add(pDownTwoButton.GetComponent<ButtonPowerDownScript>());
            powerDownButtonScripts.Add(pDownThreeButton.GetComponent<ButtonPowerDownScript>());
            powerDownButtonScripts.Add(pDownFourButton.GetComponent<ButtonPowerDownScript>());
            powerDownButtonScripts.Add(pDownFiveButton.GetComponent<ButtonPowerDownScript>());

            powerUpButtonScripts.Add(pUpOneButton.GetComponent<ButtonPowerUpScript>());
            powerUpButtonScripts.Add(pUpTwoButton.GetComponent<ButtonPowerUpScript>());
            powerUpButtonScripts.Add(pUpThreeButton.GetComponent<ButtonPowerUpScript>());
            powerUpButtonScripts.Add(pUpFourButton.GetComponent<ButtonPowerUpScript>());
            powerUpButtonScripts.Add(pUpFiveButton.GetComponent<ButtonPowerUpScript>());

        }


    }
}
