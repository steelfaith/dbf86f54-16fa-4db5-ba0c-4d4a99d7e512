using System;
using System.ComponentModel;
using Assets.ServerStubHome.Monsters;

namespace Assets.Infrastructure
{
    public enum MonsterList
    {
        [Description("unitychan")]
        unitychan,
        [Description("Demon Enforcer")]
        DemonEnforcer,
        [Description("Rhino Virus")]
        RhinoVirus,
        [Description("Robot Shock Trooper")]
        RobotShockTrooper,
        [Description("Green Spider")]
        GreenSpider,
        [Description("Dragonling")]
        Dragonling,
        [Description("Humpback Whale")]
        Humpback,
        [Description("Tripod")]
        Tripod,
        [Description("Mini Land Shark")]
        MiniLandShark
    }

    public static class MonsterListToConcreteImplementationActionExtension
    {
        public static Func<IMonsterDna> GetConcreteClass(this MonsterList value)
        {
            switch (value)
            {
                case MonsterList.DemonEnforcer:
                    return new Func<IMonsterDna>(() => { return new DemonEnforcer(); });
                    
            }
            return null;
        }
    }
}
