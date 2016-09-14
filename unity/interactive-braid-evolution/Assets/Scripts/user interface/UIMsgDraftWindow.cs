using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class UIMsgDraftWindow : MonoBehaviour {

    public KeyCode debugKey = KeyCode.T;

    private bool toggle = true;
    private GameObject msgWindow;
    private Text text;

    void Start()
    {
        msgWindow = GameObject.FindGameObjectWithTag("UIMsgDraftWindow");
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
        text.text += v + "\n";
    }
}
