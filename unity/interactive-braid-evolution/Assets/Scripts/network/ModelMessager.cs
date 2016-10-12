using UnityEngine;
using System.Collections;

public class ModelMessager : MonoBehaviour {

    // Network variables
    private UDPSender sender;
    private UDPMessage msg;

    // UI
    //TODO: Tidu up this ui 
    private UIMsgWindow msgWindow;
    private UIMsgDraftWindow msgDraftWindow;
    private bool hasUI;

    // Message variables
    private int m_populationSize = 1;
    private int m_height = 1; 
    private Vector3[] m_messageVectors;


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

    public void SetupEvolutionParameters(int populationSize, int height)
    {
        m_populationSize = populationSize;
        m_height = height;
        Debug.Log("Evolution parameters set in network messsenger");
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

        msg.height = m_height;
        msg.population_size = m_populationSize;

        //Vector3[] vects = {
        //    new Vector3(0, 0, 0),
        //    new Vector3(1, 0, 2),
        //    new Vector3(1, 0, 4),
        //    new Vector3(3, 0, 6)
        //};

        msg.vectors = m_messageVectors; 

        string s = JsonUtility.ToJson(msg);

        if(hasUI)
            msgWindow.AddMessage("Message sent to GH");

        Debug.Log("ModelMsg: " + s);
        sender.SendString(s);
    }
}
