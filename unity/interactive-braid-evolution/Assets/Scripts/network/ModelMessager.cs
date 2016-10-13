﻿using UnityEngine;
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
    private Vector3[][] m_messageVectors;


    [Serializable]
    public class UDPMessage
    {
        public int height; 
        public int population_size;
        public Vector3[][] vectors; 
    }

    void Start () {

       

        if (GameObject.FindGameObjectWithTag("UIManager"))
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
        m_messageVectors = new Vector3[populationSize][];
        Debug.Log("Evolution parameters set in network messsenger");
    }

    public void PrintCurrentVectors()
    {
        int listIndex = 0;
        Debug.Log("************* MESSAGEOBJECT VECTOR3[][] *****************");
        foreach (Vector3[] vList in m_messageVectors) {

            if (vList != null)
            {
                Debug.Log("************* LIST " + listIndex + " *****************");
                foreach (Vector3 v in vList)
                    Debug.Log(v);
            }
            else
            {
                Debug.Log("No vectors in list " + listIndex);
            }
            listIndex++; 
        }
        Debug.Log("************* MESSAGEOBJECT END ****************");
        Debug.Log(" "); 

    }

    public void AddVectors(int index, Vector3[] vectors)
    {
        Debug.Log("recieved vectors: ");
        foreach (Vector3 v in vectors)
            Debug.Log(v);

        m_messageVectors[index] = vectors;

        PrintCurrentVectors(); 
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
