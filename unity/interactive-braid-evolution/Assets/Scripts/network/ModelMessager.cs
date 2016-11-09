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

    public void SetupEvolutionParameters(int populationSize, int height)
    {
        m_populationSize = populationSize;
        m_height = height;
        m_messageVectors = new Vector3[populationSize][];
        //Debug.Log("Evolution parameters set in network messsenger");
        
    }

    public void AddVectors(int index, Vector3[] vectors)
    {
        m_messageVectors[index] = vectors;
    }

    public void SendMessageToGH()
    {
        m_populationSize = 5;
        IECManager.SetUIToModellingState();
        Braid[] braids = CreateBraidArray(m_messageVectors);

         
        string s = JsonHelper.CreateJSONFromBraids(m_height, m_populationSize, braids); 

        sender.SendString(s);
    }

    public Braid[] CreateBraidArray(Vector3[][] braidVectors)
    {
        Braid[] braids = new Braid[m_populationSize];

        for (int i = 0; i < braids.Length; i++)
        {
            Braid b;
            //Vector3[] vects = {
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4), 0),
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,2),
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,4),
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,6),
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,8),
            //    new Vector3( Random.Range(i, i + 4), Random.Range(i, i + 4) ,10)
            //};

            if (m_messageVectors[i] != null)
            {
                b = new Braid("braid_" + i.ToString(), braidVectors[i]);
                braids[i] = b;
            }
            //} else
            //{
            //    if(i == 1)
            //        b = new Braid("braid_" + i.ToString(), vects);
            //    else
            //        b = new Braid("braid_" + i.ToString(), vects);
            //}


        }

        return braids; 
    }
}
