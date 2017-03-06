using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Load: MonoBehaviour    
    {
        public static Load _loadScript;
        public int _scene;
        public bool _loaded;

        private void Awake()
        {
            _loadScript = this;
        }

        private void OnTriggerEnter()
        {
            if (!_loadScript._loaded)
            {
                AnyManager._anyManager.LoadScene(_scene);                
                _loaded = true;
            }
        }
    }
}
