using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class CITARepresentationTester : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize;
    private Braid[] m_braidList;
    private int m_id; 

    void Start()
    {
        sender = GameObject.FindObjectOfType<UDPSender>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            SendMessageToGH(); 
    }

    public void SendMessageToGH()
    {
        Vector3[] vects = new Vector3[9];
        for (int i = 0; i < vects.Length; i++)
            vects[i] = new Vector3(0.0f, i, 0.0f);


        var jo = new JObject();
        jo.Add("id", m_id++);
        jo.Add("vectors", JToken.FromObject(vects));
        var s = jo.ToString();

        Debug.Log(s);
        sender.SendString(s);
    }
}
