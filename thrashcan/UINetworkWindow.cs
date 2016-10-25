using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UINetworkWindow : MonoBehaviour {

    public KeyCode debugKey = KeyCode.T;

    private bool toggle = true;
    private GameObject networkWindow;
    private Text text;

    //TODO: Export as logfile
    void Awake()
    {
        networkWindow = GameObject.FindGameObjectWithTag("UINetworkWindow");
        text = networkWindow.GetComponentInChildren<Text>();
        text.text += "\n";
    }

    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            networkWindow.SetActive(!toggle);
            toggle = !toggle;
        }
    }

    public void AddMessage(string v)
    {
        text.text += v + "\n"; 
    }
}
