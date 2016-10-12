using UnityEngine;
using System.Collections;

public class ModelMessager : MonoBehaviour {

    private UDPSender sender;
    private UDPMessage msg;

    private UIMsgWindow msgWindow;
    private UIMsgDraftWindow msgDraftWindow;

    private Vector3[] m_messageVectors;
    private bool hasUI; 

    [Serializable]
    public class UDPMessage
    {
        public int height; 
        public int population_size;
        public Vector3[] vectors; 
    }

    void Start () {

        if(GameObject.FindGameObjectWithTag("UIManager"))
        {
            msgWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgWindow>();
            msgDraftWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgDraftWindow>();
            hasUI = true; 
        } else
        {
            hasUI = false; 
            Debug.LogWarning("No UI interface detected"); 
        }

        sender = Camera.main.GetComponent<UDPSender>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SendMessageToGH();
        }
    }

    public void SetupVectors(Vector3[] vectors)
    {
        m_messageVectors = vectors;
        Debug.Log("Vectors have been set up for the message, and m_messageVectors is now: ");

        foreach (Vector3 v in m_messageVectors)
            Debug.Log(v); 
    }

    public void SendMessageToGH()
    {

        UDPMessage msg = new UDPMessage();
        //int[] values = msgDraftWindow.GetParams();

        msg.height = 5;
        msg.population_size = 8;

        Vector3[] vects = {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 2),
            new Vector3(1, 0, 4),
            new Vector3(3, 0, 6)
        };

        msg.vectors = vects; 

        string s = JsonUtility.ToJson(msg);

        if(hasUI)
            msgWindow.AddMessage("Message sent to GH");

        Debug.Log("ModelMsg: " + s);
        sender.SendString(s);
    }
}
