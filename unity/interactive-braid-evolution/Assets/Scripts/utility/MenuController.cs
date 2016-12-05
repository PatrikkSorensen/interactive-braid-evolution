using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    private static Animator anim;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Animator>();

    }

    public static void ToggleMenu()
    {
        anim.SetTrigger("fade");
    }
    
    public void ToggleShortcuts()
    {
        anim.SetTrigger("shortcuts");
    }
}
