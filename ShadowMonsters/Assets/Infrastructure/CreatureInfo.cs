using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Infrastructure
{
    public class CreatureInfo
    {
        private MonsterList _monsterValue;
        public CreatureInfo(MonsterList value)
        {
            _monsterValue = value;
        }

        public string NameKey
        {
            get { return _monsterValue.ToString(); }
        }
        public string DisplayName
        {
            get { return EnumHelper<MonsterList>.GetEnumDescription(NameKey); }
        }
        public int Level { get; set; }
        
    }
}
