using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelMessenger : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize;
    private Braid[] m_braidList; 

    void Start () {
        sender = GameObject.FindObjectOfType<UDPSender>(); 
    }

    public void SetupEvolutionParameters(int populationSize)
    {
        m_populationSize = populationSize;
        m_braidList = new Braid[populationSize]; 
    }

    public void SendMessageToGH()
    {
        IECManager.SetUIToModellingState(m_populationSize);
        string s = JsonHelper.CreateJSONFromBraids(m_populationSize, m_braidList);

        Debug.Log(s);
        
        sender.SendString(s);
    }

    public void AddBraid(Braid b, int id)
    {
        if (id > m_braidList.Length)
            Debug.Log("Error: Braid array out or range: " + id); 
        m_braidList[id] = b; 
    }
}
