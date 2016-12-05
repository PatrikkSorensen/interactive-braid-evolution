using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {

    private void Start()
    {
        Debug.Log("Scene manager here"); 
    }
    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(SplashScreenManager.sceneIndex); 
    }

    public void LoadSceneFromIndex(int i)
    {
        SceneManager.LoadScene(i);
    }

}
