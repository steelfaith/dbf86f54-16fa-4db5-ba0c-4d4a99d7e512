using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class PlayerData
    {
        public List<MonsterInfo> CurrentTeam { get; set; }
        public List<Guid> AttackIds { get; set; }
        
    }
}
