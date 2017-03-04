using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;

namespace Assets
{
    public static class ServerStub
    {
        public static CreatureInfo GetRandomMonster()
        {
            var nameKey = GetRandomKey();
            return new CreatureInfo
            {
                DisplayName = EnumHelper<MonsterList>.GetEnumDescription(nameKey),
                NameKey = nameKey
            };
        }

        private static string GetRandomKey()
        {
            var list = Enum.GetNames(typeof(MonsterList)).ToList();

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
