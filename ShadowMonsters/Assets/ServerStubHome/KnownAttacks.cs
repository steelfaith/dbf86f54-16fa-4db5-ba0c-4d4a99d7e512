﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Infrastructure;
using UnityEngine;

namespace Assets.ServerStubHome
{

    public class KnownAttacks
    {
        public KnownAttacks()
        {
            KnownMonsterAttackList = new Dictionary<Guid, AttackInfo>();
            KnownPlayerAttackList = new Dictionary<Guid, AttackInfo>();
            AllKnownAttackList = new Dictionary<Guid, AttackInfo>();
            CreateMonsterAttacks();
            CreatePlayerAttacks();
        }

        public Dictionary<Guid,AttackInfo> KnownMonsterAttackList { get; set; }
        public Dictionary<Guid,AttackInfo> KnownPlayerAttackList { get; set; }
        public Dictionary<Guid, AttackInfo> AllKnownAttackList { get; set; }

        private void CreateMonsterAttacks()
        {

            var attackList = new List<AttackInfo>
            {
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Fae Ball",
                    DamageStyle = DamageStyle.Delayed,
                    Affinity =ElementalAffinity.Fae,
                    CastTime = 3,
                    Cooldown = 0,
                    BaseDamage = 50,
                    Icon = Resources.Load<Sprite>("fireball") ,
                    CanPowerUp = true,
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Doom Bolt",
                    DamageStyle = DamageStyle.Delayed,
                    Affinity =ElementalAffinity.Demon,
                    CastTime = 5,
                    Cooldown = 0,
                    BaseDamage = 70,
                    Icon = Resources.Load<Sprite>("bolt"),
                    CanPowerUp = true,
                },
                 new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Axe Flurry",
                    DamageStyle = DamageStyle.Tick,
                    Affinity =ElementalAffinity.Mechanical,
                    CastTime = 0,
                    Cooldown = 5,
                    BaseDamage =15,
                    Icon =  Resources.Load<Sprite>("axe"),
        },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Air Jab",
                    DamageStyle = DamageStyle.Instant,
                    Affinity = ElementalAffinity.Wind,
                    CastTime = 0,
                    Cooldown = 2,
                    BaseDamage = 35,
                    Icon = Resources.Load<Sprite>("fist"),
                    IsGenerator = true,
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Wing Smash",
                    DamageStyle = DamageStyle.Instant,
                    Affinity =ElementalAffinity.Fae,
                    CastTime = 0,
                    Cooldown = 4,
                    BaseDamage = 60,
                    Icon = Resources.Load<Sprite>("wings"),
                    IsGenerator = true,
                }
            };

            foreach (AttackInfo info in attackList)
            {
                KnownMonsterAttackList[info.AttackId] = info;
                AllKnownAttackList[info.AttackId] = info;
            }
        }
        private void CreatePlayerAttacks()
        {
            var attackList = new List<AttackInfo>
            {
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Scream",
                    DamageStyle = DamageStyle.Tick,
                    Affinity =ElementalAffinity.Human,
                    CastTime = 0,
                    Cooldown = 5,
                    BaseDamage = 5
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Kick",
                    DamageStyle = DamageStyle.Instant,
                    Affinity =ElementalAffinity.Human,
                    CastTime = 0,
                    Cooldown = 5,
                    BaseDamage = 15
                },
                 new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Punch",
                    DamageStyle = DamageStyle.Instant,
                    Affinity =ElementalAffinity.Human,
                    CastTime = 0,
                    Cooldown = 3,
                    BaseDamage =10
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Clout",
                    DamageStyle = DamageStyle.Delayed,
                    Affinity = ElementalAffinity.Human,
                    CastTime = 5,
                    Cooldown = 0,
                    BaseDamage = 20
                },
                new AttackInfo
                {
                    AttackId = Guid.NewGuid(),
                    Name = "Tackle",
                    DamageStyle = DamageStyle.Instant,
                    Affinity =ElementalAffinity.Human,
                    CastTime = 0,
                    Cooldown = 6,
                    BaseDamage = 25
                }
            };

            foreach (AttackInfo info in attackList)
            {
                KnownPlayerAttackList[info.AttackId] = info;
                AllKnownAttackList[info.AttackId] = info;
            }
        }
    }
}