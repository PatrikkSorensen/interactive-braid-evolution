using UnityEngine;
using System.Collections;
using System;

public class BraidSimulationManager : MonoBehaviour {

    public static int vectorArraysMade; 
    public static int evaluationsMade; 
    public static int populationSize;

    private void Start()
    {
        vectorArraysMade = 0;
        evaluationsMade = 0; 
    }

    public void SetFlagsFromButton(bool b)
    {
        BraidSelector.SetShouldEvaluate(b);
    }
    


	public static void AdvanceGeneration()
    {
        // the actual models
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        
        foreach(GameObject braid in braids)
            Destroy(braid);

        // the ann controllers
        BraidSelector.SetShouldEvaluate(true);

        // variable resetting
        ResetSimulationValues();
        IECManager.SetUIToModellingState(populationSize);

        Debug.Log("Simulation reset and cleaned up, ready for the next one");
    }

    public static bool HasControllersEvaluated()
    {
        return (evaluationsMade == populationSize);
    }


    internal static bool HasControllersCreatedData()
    {
        Debug.Log(vectorArraysMade + ", " + populationSize); 
        return (vectorArraysMade == populationSize); 
    }

    public static void ResetSimulationValues()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables();
        vectorArraysMade = 0;
        evaluationsMade = 0;
    }
}
