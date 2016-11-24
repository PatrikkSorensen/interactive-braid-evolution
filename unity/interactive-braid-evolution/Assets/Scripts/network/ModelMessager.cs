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

    public void SetupEvolutionParameters(int populationSize)
    {
        m_populationSize = populationSize;
        m_height = UISliderUpdater.GetValue();
        m_messageVectors = new Vector3[populationSize][];
    }



    public void SendRandomBraidArrays(int populationSize)
    {
        IECManager.SetUIToModellingState(populationSize);
        Braid[] braids = CreateRandomBraidArray(m_populationSize); 

        string s = JsonHelper.CreateJSONFromBraids(m_height, populationSize, braids);

        sender.SendString(s);
    }

    public void SendMessageToGH()
    {
        IECManager.SetUIToModellingState(m_populationSize);
        
        Braid[] braids = CreateBraidArray(m_messageVectors);

        string s = JsonHelper.CreateJSONFromBraids(m_height, m_populationSize, braids);
        Debug.Log(s);
        
        sender.SendString(s);
    }

    public Braid[] CreateBraidArray(Vector3[][] braidVectors)
    {
        Braid[] braids = new Braid[m_populationSize];

        for (int i = 0; i < braids.Length; i++)
        {
            Braid b = new Braid("braid_" + i.ToString(), braidVectors[i]);
            braids[i] = b;
        }

        return braids; 
    }

    public static Braid[] CreateRandomBraidArray(int populationSize)
    {
        Braid[] braids = new Braid[populationSize];
        float multiplier = (float)UISliderUpdater.GetValue(); 
        for (int i = 0; i < braids.Length; i++)
        {
            Vector3[] vects = UtilityHelper.CreateRandomVectors(0, 5, 6, 2);  

            Braid b = new Braid("braid_" + i.ToString(), vects);
            braids[i] = b; 
        }

        return braids;
    }

    public void AddVectors(int index, Vector3[] vectors)
    {
        m_messageVectors[index] = vectors;
    }

    public Vector3[] GetVectors(int index)
    {
        return m_messageVectors[index];
    }
}
