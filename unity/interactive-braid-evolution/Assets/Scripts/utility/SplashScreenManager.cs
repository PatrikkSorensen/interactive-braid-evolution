using UnityEngine;
using System.Collections;

public class SplashScreenManager : MonoBehaviour {

    private Animator anim;
    public static int sceneIndex; 

    void Start()
    {
        sceneIndex = 0; 
        anim = GameObject.Find("SplashCanvas").GetComponent<Animator>(); 
    }

	public void ChangeLevel(int levelIndex)
    {
        sceneIndex = levelIndex; 
        anim.SetTrigger("fadeout");
    }
}
