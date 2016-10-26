using UnityEngine;
using System.Collections;


public class ModelMessager : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize = 1;
    private int m_height = 1; 
    private Vector3[][] m_messageVectors;

    void Start () {
        sender = GameObject.FindObjectOfType<UDPSender>(); 
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
        //Debug.Log("Evolution parameters set in network messsenger");
        
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
        //Debug.Log("recieved vectors: ");
        //foreach (Vector3 v in vectors)
        //    Debug.Log(v);

        m_messageVectors[index] = vectors;
    }

    public void SendMessageToGH()
    {
        StatusWindow.SetStatus(StatusWindow.STATUS.MODELLING);
        Braid[] braids = CreateBraidArray(m_messageVectors);

    
        string s = JsonHelper.CreateJSONFromBraids(m_height, m_populationSize, braids); 

        Debug.Log("ModelMsg: " + s);
        sender.SendString(s);
    }

    public Braid[] CreateBraidArray(Vector3[][] braidVectors)
    {
        Braid[] braids = new Braid[m_populationSize];

        if (m_messageVectors.Length == 0)
        {
            Debug.LogError("No message vectors found to populate braids."); 
            return braids; 
        } else
        {
            for (int i = 0; i < braids.Length; i++)
            {
                Braid b = new Braid("braid_" + i.ToString(), braidVectors[i]);
                braids[i] = b;
            }

        }
        return braids; 
    }
}
