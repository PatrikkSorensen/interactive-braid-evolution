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
        m_messageVectors[index] = vectors;
    }

    public void SendMessageToGH()
    {
        m_populationSize = 5;
        StatusWindow.SetStatus(StatusWindow.STATUS.MODELLING);
        Braid[] braids = CreateBraidArray(m_messageVectors);

         
        string s = JsonHelper.CreateJSONFromBraids(m_height, m_populationSize, braids); 

        Debug.Log("ModelMsg: " + s);
        sender.SendString(s);
    }

    public Braid[] CreateBraidArray(Vector3[][] braidVectors)
    {
        Braid[] braids = new Braid[m_populationSize];

        // TEST VECTORS: 
       // Vector3[] vects = {
       //     new Vector3(0, 0, 0),
       //     new Vector3(1, 0, 2),
       //     new Vector3(1, 0, 4),
       //     new Vector3(5, 0, 6)
       // };

       // Vector3[] vects2 = {
       //    new Vector3( 0,0,0),
       //    new Vector3( 0,0,2),
       //    new Vector3( 0,0,4),
       //    new Vector3( 0,0,6),
       //    new Vector3( 0,0,8),
       //    new Vector3( 2,0,10)
       //};


        for (int i = 0; i < braids.Length; i++)
        {
            Braid b;
            Vector3[] vects = {
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4), 0),
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,2),
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,4),
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,6),
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,8),
                new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,10)
            };

            if (m_messageVectors[i] != null)
            {
                b = new Braid("braid_" + i.ToString(), braidVectors[i]);
            } else
            {
                if(i == 1)
                    b = new Braid("braid_" + i.ToString(), vects);
                else
                    b = new Braid("braid_" + i.ToString(), vects);
            }

            braids[i] = b;
        }

        return braids; 
    }
}
