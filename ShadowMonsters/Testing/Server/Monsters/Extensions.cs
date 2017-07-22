using System;
using Common.Enums;
using Common.Interfaces;

namespace Server.Monsters
{
    public static class MonsterListToConcreteImplementationActionExtension
    {
        public static Func<IMonsterDna> GetConcreteClass(this MonsterList value)
        {
            switch (value)
            {
                case MonsterList.DemonEnforcer:
                    return new Func<IMonsterDna>(() => { return new DemonEnforcer(); });
                case MonsterList.Dragonling:
                    return new Func<IMonsterDna>(() => { return new Dragonling(); });
                case MonsterList.RobotShockTrooper:
                    return new Func<IMonsterDna>(() => { return new RobotShockTrooper(); });
                case MonsterList.GreenSpider:
                    return new Func<IMonsterDna>(() => { return new GreenSpider(); });
                case MonsterList.Humpback:
                    return new Func<IMonsterDna>(() => { return new Humpback(); });
                case MonsterList.MiniLandShark:
                    return new Func<IMonsterDna>(() => { return new MiniLandShark(); });
                case MonsterList.RhinoVirus:
                    return new Func<IMonsterDna>(() => { return new RhinoVirus(); });
                case MonsterList.Tripod:
                    return new Func<IMonsterDna>(() => { return new Tripod(); });

            }
            return null;
        }
    }
}
