using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class PlayerData
    {
        public Guid Id { get; set; }
        public List<MonsterInfo> CurrentTeam { get; set; }
        public List<Guid> AttackIds { get; set; }
        public string DisplayName { get; set; }
        public float CurrentHealth { get; set; }
        public float MaximumHealth { get; set; }

    }
}
