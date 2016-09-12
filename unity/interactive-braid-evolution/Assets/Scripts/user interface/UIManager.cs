using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public KeyCode key = KeyCode.R;

    private GameObject ui; 
    private bool toggle = true;   
	// Use this for initialization
	void Start () {
        ui = GameObject.FindGameObjectWithTag("UI"); 
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(key))
        {
            ui.SetActive(!toggle); 
            toggle = !toggle; 
        } 
	}
}
