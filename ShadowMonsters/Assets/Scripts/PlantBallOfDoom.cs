using Assets.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlantBallOfDoom : MonoBehaviour, IMonster
    {

        public string Name { get; set; }
        // Use this for initialization
        void Awake()
        {
            Name = "Plant Ball of Doom";
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
