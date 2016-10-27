using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class UISelectionWindow : MonoBehaviour {

    // For multiple selection
    public static string[] names = new string[10];
    public static GameObject current_selected; 

	public static void AddBraid(GameObject gb)
    {
        Text t = GameObject.FindObjectOfType<UISelectionWindow>().gameObject.GetComponent<Text>();
        t.text = "Current selection: \n" + gb.name;
        names[0] = gb.name;
        current_selected = gb; 
    }
}
