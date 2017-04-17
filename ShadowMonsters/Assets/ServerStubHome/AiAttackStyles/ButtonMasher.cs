using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;


namespace Assets.ServerStubHome.AiAttackStyles
{
    public class ButtonMasher : IAiAttackStyle
    {
        public List<AttackInfo> Attacks { get; set; }
        private List<ElementalAffinity> _resources = new List<ElementalAffinity>();
        private AttackInstance attackInstance;
        Random random = new Random();

        public ButtonMasher(AttackInstance instance)
        {
            _resources = new List<ElementalAffinity>();
            attackInstance = instance;
        }

        public void AddResource(ElementalAffinity resource)
        {
            if (_resources.Count < 6)
                _resources.Add(resource);
        }

        public AttackInfo ChooseAttack()
        {
            
            int attackIndex = random.Next(0, 5);
            var attack = Attacks[attackIndex];

            if(attack.CanPowerUp && _resources.Where(x=>x ==attack.Affinity).Count() > 2)
            {
                attack.PowerLevel = 3;
                for (int i = 0; i < 3; i++)
                {
                    _resources.Remove(attack.Affinity);
                }
                attackInstance.enemyResourceDisplayUpdateQueue.Enqueue(new EnemyResourceDisplayUpdate
                {
                    PlayerId = attackInstance.playerId,
                    Resources = _resources,
                });
            }

            return attack;
        }

        public List<ElementalAffinity> GetResources()
        {
            return _resources;
        }
    }
}
