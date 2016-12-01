using UnityEngine;
using System.Collections;

public class ChangeBraidMaterial : MonoBehaviour {

    public GameObject b1;
    public GameObject b2;
    public GameObject b3;

    public Material newMaterial;

    private Material startMaterial;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            ChangeMaterialOfBraid1(); 
    }
    private void Start()
    {
        startMaterial = b1.GetComponentInChildren<Renderer>().material; 
    }

    public void ChangeMaterialOfBraid1()
    {
        b1.GetComponentInChildren<Renderer>().material = newMaterial;
        Debug.Log("Hello world"); 
    }

    public void ChangeMaterialOfBraid2()
    {
        b2.GetComponentInChildren<Renderer>().material = newMaterial;
        Debug.Log("Changing material");
    }

    public void ChangeMaterialOfBraid3()
    {
        b3.GetComponentInChildren<Renderer>().material = newMaterial;
        Debug.Log("Changing material");
    }
}
