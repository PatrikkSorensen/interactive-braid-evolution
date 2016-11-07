using UnityEngine;
using SharpNeat.EvolutionAlgorithms; 
using System.Collections;
using DG.Tweening; 

public class BraidSelector : MonoBehaviour {

    public static bool ReadyToProgressEvolution;
    public static bool ShouldSimulateGenomes;

    void Awake()
    {
        ShouldSimulateGenomes = true; 
        ReadyToProgressEvolution = false; 
    }
	void Update () {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Braid")
            {
                //Debug.Log("HOVERING");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Braid")
                {
                    UISelectionWindow.AddBraid(hit.transform.gameObject); 
                }
            }
        }            
    }

    public static void CreateHardcodedFitness()
    {
        //BraidController bc = Object.FindObjectOfType<BraidController>();
        int id = IECManager.GetSelectionId() + 1;
        string name = "unit_" +  id.ToString();

        if (GameObject.Find(name))
        {
            BraidController bc = GameObject.Find(name).GetComponent<BraidController>();
            bc.SetFitness(1.0f);
            Debug.Log("fitness applied to: " + bc.transform.name + "...");
        } else
        {
            Debug.Log("Couldt find bc on: " + name); 
        }

    }

    public static bool ReadyForSelection()
    {
        return ReadyToProgressEvolution; 
    }

    public static void SetReadyForSelection(bool b)
    {
        ReadyToProgressEvolution = b;
    }

    public static bool ShouldEvaluate()
    {
        return ShouldSimulateGenomes;
    }

    public static void SetShouldEvaluate(bool b)
    {
        ShouldSimulateGenomes = b;
    }
}
