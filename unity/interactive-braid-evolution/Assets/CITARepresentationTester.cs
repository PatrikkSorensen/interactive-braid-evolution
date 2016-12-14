using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic; 

public class CITARepresentationTester : MonoBehaviour {

    // Network variables
    private UDPSender sender;

    // Message variables
    private int m_populationSize;
    private Braid[] m_braidList;
    private int m_id;
    private int counter; 

    void Start()
    {
        counter = 0; 
        sender = GameObject.FindObjectOfType<UDPSender>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PerformBranchedBraidsTest(); 
            //PerformSimpleVectorListTest();
        }
    }

    public void PerformSimpleVectorListTest()
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

    public void PerformBranchedBraidsTest()
    {
        Debug.Log("Performing branched braid test, counter: " + counter); 
        if (counter >= 9)
            return; 

        List<Vector3[]> braidVectors = new List<Vector3[]>();

        // Branch 1:
        Vector3[] branch1 = new Vector3[6]; 
        for(int  i = 0; i < branch1.Length; i++)
        {
            branch1[i] = new Vector3(0.0f, i * 2, 0.0f); 
        }
        branch1[0] = new Vector3(2.0f, 0.0f, 0.0f);

        // Branch 2:
        Vector3[] branch2 = new Vector3[3];
        branch2[0] = new Vector3(0.0f, 2.0f, 0.0f);
        branch2[1] = new Vector3(2.0f, 4.0f, 0.0f);
        branch2[2] = new Vector3(2.0f, 6.0f, 0.0f);

        // Branch 3: 
        Vector3[] branch3 = new Vector3[4];
        branch2[0] = new Vector3(0.0f, 6.0f, 0.0f);
        branch2[1] = new Vector3(-2.0f, 8.0f, 0.0f);
        branch2[2] = new Vector3(-2.0f, 10.0f, 0.0f);
        branch2[2] = new Vector3(-2.0f, 12.0f, 0.0f);

        braidVectors.Add(branch1);
        braidVectors.Add(branch2);
        braidVectors.Add(branch3);

        Braid b = new Braid("cita_braid", braidVectors);

        var jo = new JObject();
        jo.Add("id", m_id++);
        jo.Add("braid", JToken.FromObject(b));
        var s = jo.ToString();

        Debug.Log(s);
        sender.SendString(s);
        counter++; 
    }
}
