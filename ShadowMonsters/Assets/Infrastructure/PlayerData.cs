using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class PlayerData
    {
        public Guid Id { get; set; }
        public List<IMonsterDna> CurrentTeam { get; set; }
        public List<Guid> AttackIds { get; set; }
        public IMonsterDna PlayerDna { get; set; }

    }
}
