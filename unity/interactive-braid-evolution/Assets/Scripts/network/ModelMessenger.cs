using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelMessenger : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize;
    //private int m_height = 1; 

    //private Vector3[][] m_messageVectors;
    private double[][] m_materialValues;
    private double[][] m_radiusValues;
    private Braid[] m_braidList; 

    void Start () {
        sender = GameObject.FindObjectOfType<UDPSender>(); 
    }

    public void SetupEvolutionParameters(int populationSize)
    {
        m_populationSize = populationSize;
        //m_height = UISliderUpdater.GetValue();
        //m_messageVectors = new Vector3[populationSize][];
        m_materialValues = new double[populationSize][]; 
        m_radiusValues = new double[populationSize][];
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
        m_braidList[id] = b; 
    }

    //public void AddVectors(int index, Vector3[] vectors)
    //{
    //    m_messageVectors[index] = vectors;
    //}

    public void AddMaterialArray(int index, double[] array)
    {
        
        m_materialValues[index] = array; 
    }

    public void AddRadiusArray(int index, double[] array)
    {
        m_radiusValues[index] = array;
    }

    //public Vector3[] GetVectors(int index)
    //{
    //    return m_messageVectors[index];
    //}
}
