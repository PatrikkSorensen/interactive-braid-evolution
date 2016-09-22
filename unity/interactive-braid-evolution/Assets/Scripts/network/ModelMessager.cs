using UnityEngine;
using System.Collections;

public class ModelMessager : MonoBehaviour {

    private UDPSender sender;
    private UDPMessage msg;

    private UIMsgWindow msgWindow;
    private UIMsgDraftWindow msgDraftWindow; 


    [Serializable]
    public class UDPMessage
    {
        public int thickness;
        public int height; 
        public int novelty;
    }

    void Start () {
        msgWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgWindow>(); 
        msgDraftWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgDraftWindow>();
        sender = Camera.main.GetComponent<UDPSender>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SendMessageToGH();
        }
    }

    public void SendMessageToGH()
    {
        UDPMessage msg = new UDPMessage();
        int[] values = msgDraftWindow.GetParams();
        msg.height = values[0];
        msg.novelty = values[1];
        msg.thickness = values[2];
        string s = JsonUtility.ToJson(msg);

        msgWindow.AddMessage("Message sent to GH");
        Debug.Log("ModelMsg: " + s);
        sender.SendString(s);
    }
}
