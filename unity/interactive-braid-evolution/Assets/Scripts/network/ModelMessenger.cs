using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelMessenger : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize;
    private Braid[] m_braidList;

    // new approach: 
    List<Braid> braids = new List<Braid>();
    bool m_modelling;
    public bool modelling
    {
        get { return m_modelling; }
        set { m_modelling = value; }
    }

    bool StartSendingMessages; 

    void Start () {
        StartSendingMessages = false; 
        modelling = false; 
        sender = GameObject.FindObjectOfType<UDPSender>(); 
    }

    private void Update()
    {
        if (!modelling && StartSendingMessages && braids.Count != 0)
            SendBraidToGH(); 
    }

    public void SetupEvolutionParameters(int populationSize)
    {
        m_populationSize = populationSize;
        m_braidList = new Braid[populationSize]; 
    }

    public void SendBraidToGH()
    {
        //Debug.Log("Sending braid!"); 
        string message = JsonHelper.CreateJSONFromSingleBraid(braids[0]);
        //Debug.Log(message);
        sender.SendString(message);
        modelling = true; 
        braids.RemoveAt(0); 
    }

    public void SendMessageToGH()
    {
        Debug.Log("Starting to send messages..."); 
        StartSendingMessages = true;
        IECManager.SetUIToModellingState(m_populationSize);

        //string s = JsonHelper.CreateJSONFromBraids(m_populationSize, m_braidList);
        //Debug.Log(s);
        //sender.SendString(s);
    }

    public void AddBraid(Braid b, int id)
    {
        //Debug.Log("Added data for braid!"); 
        //if (id > m_braidList.Length)
        //    Debug.Log("Error: Braid array out or range: " + id); 
        //m_braidList[id] = b;

        braids.Add(b); 
    }
}
