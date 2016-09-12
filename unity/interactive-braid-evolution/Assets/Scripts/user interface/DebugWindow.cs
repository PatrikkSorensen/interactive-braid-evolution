using UnityEngine;
using System.Collections;

public class DebugWindow : MonoBehaviour {

    public KeyCode debugKey = KeyCode.T;

    private bool toggle = true; 
    private GameObject debugWindow; 
	
	void Start () {
        debugWindow = GameObject.FindGameObjectWithTag("DebugWindow");     
	}

    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            Debug.Log("Toggle: " + toggle); 
            debugWindow.SetActive(!toggle);
            toggle = !toggle; 
        }
    }
}
