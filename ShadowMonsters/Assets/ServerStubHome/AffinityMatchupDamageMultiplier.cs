using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;

namespace Assets.ServerStubHome
{
    public class AffinityMatchupDamageMultiplier
    {
        public AffinityMatchupDamageMultiplier()
        {
            Amdm = new Dictionary<ElementalAffinity, Dictionary<ElementalAffinity, float>>();
            SetupAmdm();
        }

        private void SetupAmdm()
        {
            Amdm[ElementalAffinity.Demon] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1.5f },
                { ElementalAffinity.Fae, .5f },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, .5f },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, 1.5f },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Dragon] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, .5f },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1.5f },
                { ElementalAffinity.Fire, 1.5f },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, .5f },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Fae] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1.5f },
                { ElementalAffinity.Dragon, .5f },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1.5f },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, .5f },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Fire] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, .5f },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1.5f },
                { ElementalAffinity.Shadow, 1.5f },
                { ElementalAffinity.Water, .5f },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Human] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Light] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1.5f },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, .5f },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, .5f },
                { ElementalAffinity.Water, 1.5f },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, 1 }
            };

            Amdm[ElementalAffinity.Mechanical] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, .5f },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1.5f },
                { ElementalAffinity.Water, .5f },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, 1.5f }
            };

            Amdm[ElementalAffinity.Shadow] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, .5f },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1.5f },
                { ElementalAffinity.Mechanical, .5f },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, 1.5f }
            };

            Amdm[ElementalAffinity.Water] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, 1.5f },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, .5f },
                { ElementalAffinity.Mechanical, 1.5f },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, .5f }
            };

            Amdm[ElementalAffinity.Wind] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, .5f },
                { ElementalAffinity.Dragon, 1.5f },
                { ElementalAffinity.Fae, 1.5f },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, 1 },
                { ElementalAffinity.Shadow, 1 },
                { ElementalAffinity.Water, 1 },
                { ElementalAffinity.Wind, 1 },
                { ElementalAffinity.Wood, .5f }
            };

            Amdm[ElementalAffinity.Wood] = new Dictionary<ElementalAffinity, float>
            {
                { ElementalAffinity.Demon, 1 },
                { ElementalAffinity.Dragon, 1 },
                { ElementalAffinity.Fae, 1 },
                { ElementalAffinity.Fire, 1 },
                { ElementalAffinity.Human, 1 },
                { ElementalAffinity.Light, 1 },
                { ElementalAffinity.Mechanical, .5f },
                { ElementalAffinity.Shadow, .5f },
                { ElementalAffinity.Water, 1.5f },
                { ElementalAffinity.Wind, 1.5f },
                { ElementalAffinity.Wood, 1 }
            };
        }

        public static Dictionary<ElementalAffinity, Dictionary<ElementalAffinity, float>> Amdm { get; set; }

        public float GetAffinityMatchupDamageMultiplier(ElementalAffinity attack, ElementalAffinity target)
        {
            Dictionary<ElementalAffinity, float> outDic;
            Amdm.TryGetValue(attack, out outDic);
            if (outDic == null) return 1;
            return outDic.FirstOrDefault(x => x.Key == target).Value;
        }

    }
}
