using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoadScene: MonoBehaviour    
    {

        public int _scene;


        private void Awake()
        {

        }

        private void OnTriggerEnter()
        {
            AnyManager._anyManager.SafeLoadScene(_scene);
        }
    }
}
