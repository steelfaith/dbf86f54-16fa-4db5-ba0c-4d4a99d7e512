using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    /// <summary>
    /// bigger refactor than i wanted to take on right now..will get back to it
    /// </summary>
    public interface IMonster
    {
        ElementalAffinity MonsterAffinity { get; set; }
        MonsterPresence MonsterPresence { get; set; }
        MonsterList MonsterListValue { get; set; }
    }
}
