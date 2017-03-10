using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Infrastructure
{
    public class BaseCreature : MonoBehaviour
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public int Health { get; set; }

    }
}
