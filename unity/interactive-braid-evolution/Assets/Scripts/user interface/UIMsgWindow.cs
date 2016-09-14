using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMsgWindow : MonoBehaviour {

    public KeyCode debugKey = KeyCode.T;

    private bool toggle = true; 
    private GameObject msgWindow;
    private Text text;  
	
    //TODO: Export as logfile
	void Awake () {
        msgWindow = GameObject.FindGameObjectWithTag("UIMsgWindow");
        text = msgWindow.GetComponentInChildren<Text>();
        text.text += "\n";
    }

    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            msgWindow.SetActive(!toggle);
            toggle = !toggle; 
        }
    }

    public void AddMessage(string v)
    {
        if (!text)
            return; 
        
        text.text += "- " + v + "\n";
    }
}
