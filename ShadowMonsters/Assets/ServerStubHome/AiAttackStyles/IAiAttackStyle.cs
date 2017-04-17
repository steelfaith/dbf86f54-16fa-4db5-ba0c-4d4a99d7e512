using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;

namespace Assets.ServerStubHome.AiAttackStyles
{
    public interface IAiAttackStyle
    {
        List<AttackInfo> Attacks { get; set; }
        AttackInfo ChooseAttack();
        void AddResource(ElementalAffinity resource);
        List<ElementalAffinity> GetResources();
    }
}
