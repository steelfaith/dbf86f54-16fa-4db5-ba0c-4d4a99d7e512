using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyManager : MonoBehaviour {

    public static AnyManager _anyManager;
    public Camera _mainCamera;
    public GameObject _player;
    const int CombatSceneIndex = 1;

    bool _gameStart;
    int _lastNonCombatSceneLoaded;


    private int LastSceneLoaded
    {
        get
        {
          return _lastNonCombatSceneLoaded  ;
        }
        set
        {
            if (value == CombatSceneIndex) return;
            _lastNonCombatSceneLoaded = value;
        }
    }


    private void Awake()
    {
        if(!_gameStart)
        {
            _anyManager = this;
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            LastSceneLoaded = 2;
            _gameStart = true;
        }

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void MainCameraOn(bool on)
    {
        _mainCamera.enabled = on ? true : false;
    }

    private void PlayerOn(bool on)
    {
        _player.SetActive(on ? true : false);
    }

    /// <summary>
    /// using this to keep track of last scene loaded
    /// </summary>
    /// <param name="_scene"></param>
    public void LoadScene(int _scene)
    {
        SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive);
        LastSceneLoaded = _scene;
    }



    public void UnloadCombatScene()
    {
        UnloadScene(CombatSceneIndex);
        MainCameraOn(true);
        PlayerOn(true);
        LoadPreviousScene();
    }

    public void LoadCombatScene()
    {
        UnloadPreviousScene();    
        LoadScene(CombatSceneIndex);       

    }

    public void UnloadScene(int scene)
    {
        StartCoroutine(Unload(scene));
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name == "CombatScene")
        {
            PlayerOn(false);
            MainCameraOn(false);
        }
    }
    private void LoadPreviousScene()
    {
        if (LastSceneLoaded == 0) return;
        SceneManager.LoadSceneAsync(LastSceneLoaded, LoadSceneMode.Additive);
    }

    private void UnloadPreviousScene()
    {
        UnloadScene(LastSceneLoaded);
    }



    private IEnumerator Unload(int scene)
    {
        yield return null;

        SceneManager.UnloadSceneAsync(scene);
    }


}
