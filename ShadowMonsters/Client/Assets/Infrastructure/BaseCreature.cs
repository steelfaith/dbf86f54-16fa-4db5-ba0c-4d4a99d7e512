﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class BaseCreature : MonoBehaviour
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public float Health { get; set; }

        public Guid MonsterId { get; set; }

    }
}