using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {


    public static void ToggleMenu()
    {
        GameObject menu = GameObject.FindGameObjectWithTag("MainMenu");
        menu.GetComponent<Animator>().SetTrigger("fade"); 
    }
}
