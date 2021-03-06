﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(SplashScreenManager.sceneIndex); 
    }

    public void LoadSceneFromIndex(int i)
    {
        SceneManager.LoadScene(i);
    }

}
