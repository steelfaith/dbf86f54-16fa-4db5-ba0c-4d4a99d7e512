using System;
using System.Collections.Generic;
using Assets.Infrastructure;

namespace Assets.ServerStubHome
{
    public static class DnaFactory
    {
        static Dictionary<MonsterList, Func<IMonsterDna>> _nameRegistrar = new Dictionary<MonsterList, Func<IMonsterDna>>();

        public static void RegisterMonster(MonsterList name)
        {
            _nameRegistrar[name] = name.GetConcreteClass();
        }

        public static IMonsterDna CreateSpecificMonsterDna(MonsterList name)
        {
            var ctor = _nameRegistrar[name];
            if (ctor != null)
                return ctor();
            return null;            
        }

    }
}
