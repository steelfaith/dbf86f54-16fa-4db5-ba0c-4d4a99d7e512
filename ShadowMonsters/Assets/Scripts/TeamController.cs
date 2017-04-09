using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using Assets.ServerStubHome;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class TeamController : MonoBehaviour, IDropHandler
    {
        public GameObject teamMember1;
        public GameObject teamMember2;
        public GameObject teamMember3;
        public GameObject teamMember4;
        public GameObject teamMember5;
        public GameObject teamMember6;
        private List<GameObject> members = new List<GameObject>(); //order matters here
        private IncarnationContainer incarnationContainer;

        private ServerStub serverStub;
        private PlayerController player;

        private void Start()
        {
            player = PlayerController.Instance();
            serverStub = ServerStub.Instance();
            incarnationContainer = IncarnationContainer.Instance();
            
            members.Add(teamMember1);
            members.Add(teamMember2);
            members.Add(teamMember3);
            members.Add(teamMember4);
            members.Add(teamMember5);
            members.Add(teamMember6);
            LoadTeam(player.GetTeam());
        }

        private static TeamController teamController;

        public static TeamController Instance()
        {
            if (!teamController)
            {
                teamController = FindObjectOfType(typeof(TeamController)) as TeamController;
                if (!teamController)
                    Debug.LogError("Could not find Team Controller");
            }
            return teamController;
        }

        public void LoadTeam(List<MonsterDna> teamMembers)
        {
            if (teamMembers == null && !teamMembers.Any()) return;

            int i = 1;
            foreach (var member in members)
            {                
                var dna = teamMembers.FirstOrDefault(x => x.TeamOrder == i);
                if (dna == null) continue;
                member.SetActive(true);
                var statusController = member.GetComponentInChildren<StatusController>();
                if (statusController == null) continue;
                statusController.SetMonster(dna.NickName, dna.Level.ToString(), dna.MonsterPresence, dna.CurrentHealth, dna.MaxHealth, dna.MonsterId);
                i++;
            }

        }

        public void UpdateLead(AttackResolution attack)
        {
            var lead = incarnationContainer.item;
            if (lead == null) return;
            var statusController = lead.GetComponentInChildren<StatusController>();
            statusController.UpdateMonster(attack);
        }

        public void HandleCodeDrop(GameObject item)
        {
            item.transform.SetParent(transform);

            var statusController = item.GetComponent<StatusController>();
            if (statusController != null)
                statusController.displayName.color = new Color32(0, 253, 0, 255);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var fatbic = FatbicDisplayController.Instance();
            if (fatbic != null && fatbic.IsBusy) return;
            DragHandler.itemBeingDragged.transform.SetParent(transform);

            var statusController = DragHandler.itemBeingDragged.GetComponent<StatusController>();
            if (statusController != null)
                statusController.displayName.color = new Color32(0, 253, 0, 255);
            var combatPlayer = CombatPlayerController.Instance();
            if (combatPlayer != null && combatPlayer.InCombat)
            {
                combatPlayer.RevertIncarnation();
            }
            
            
        }
    }
}
