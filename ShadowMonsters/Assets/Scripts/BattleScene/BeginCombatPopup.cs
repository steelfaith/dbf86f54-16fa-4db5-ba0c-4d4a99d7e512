using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

namespace Assets.Scripts
{
    public class BeginCombatPopup : MonoBehaviour
    {

        public Text _announcement;
        public Button _fight;
        public Button _run;
        public Button _bond;
        public GameObject _modalPanel;

        private static BeginCombatPopup _beginCombatPanel;

        public static BeginCombatPopup Instance()
        {
            if (!_beginCombatPanel)
            {
                _beginCombatPanel = FindObjectOfType(typeof(BeginCombatPopup)) as BeginCombatPopup;
                if (!_beginCombatPanel)
                    Debug.LogError("Could not find Begin Combat Popup");
            }
            return _beginCombatPanel;
        }

        public void PromptUserAction(string nameOfMob, UnityAction fight, UnityAction run, UnityAction bond)
        {
            _modalPanel.SetActive(true);
            _fight.onClick.RemoveAllListeners();
            _fight.onClick.AddListener(fight);
            _fight.onClick.AddListener(ClosePanel);

            _run.onClick.RemoveAllListeners();
            _run.onClick.AddListener(run);
            _run.onClick.AddListener(ClosePanel);

            _bond.onClick.RemoveAllListeners();
            _bond.onClick.AddListener(bond);
            _bond.onClick.AddListener(ClosePanel);

            _announcement.text = string.Format("A {0} has appeared! What would you like to do?",nameOfMob);

            _fight.gameObject.SetActive(true);
            _run.gameObject.SetActive(true);
            _bond.gameObject.SetActive(true);

        }

        private void ClosePanel()
        {
            _modalPanel.SetActive(false);
        }
    }
}
