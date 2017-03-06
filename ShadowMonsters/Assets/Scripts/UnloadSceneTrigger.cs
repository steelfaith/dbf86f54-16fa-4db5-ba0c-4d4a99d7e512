using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnloadSceneTrigger : MonoBehaviour
    {
        public int scene;
        bool unloaded;

        private void OnTriggerEnter()
        {
            if (!unloaded)
            {
                unloaded = true;
                AnyManager._anyManager.UnloadScene(scene);
            }
        }
    }
}
