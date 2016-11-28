using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {

    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(SplashScreenManager.sceneIndex); 
    }

}
