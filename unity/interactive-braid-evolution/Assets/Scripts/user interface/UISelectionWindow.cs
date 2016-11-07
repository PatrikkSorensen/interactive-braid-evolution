using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UISelectionWindow : MonoBehaviour {

    // For multiple selection
    public static string[] names = new string[10];
    public static GameObject current_selected; 

	public static void AddBraid(GameObject gb)
    {
        Text t = GameObject.Find("SelectionWindow").GetComponentInChildren<Text>();
        t.text = "Current selection: \n" + gb.name;
        names[0] = gb.name;
        current_selected = gb; 
    }
}
