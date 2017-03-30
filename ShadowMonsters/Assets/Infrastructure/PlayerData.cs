using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class PlayerData
    {
        public Guid Id { get; set; }
        public List<MonsterDna> CurrentTeam { get; set; }
        public List<Guid> AttackIds { get; set; }
        public MonsterDna PlayerDna { get; set; }

    }
}
