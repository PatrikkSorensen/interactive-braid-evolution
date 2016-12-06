using UnityEngine;
using System.Collections;
using System;

public class BraidSimulationManager : MonoBehaviour {

    public static int vectorArraysMade; 
    public static int evaluationsMade; 
    public static int populationSize;

    public static bool shouldSimulateGenomes;

    private void Start()
    {
        shouldSimulateGenomes = true;
        vectorArraysMade = 0;
        evaluationsMade = 0; 

    }

    public void SetBraidSimulationFromButton(bool b)
    {
        shouldSimulateGenomes = b;
    }

    public static void SetShouldBraidsEvaluate(bool b)
    {
        shouldSimulateGenomes = b;
    }

    public static bool ShouldBraidsEvaluate()
    {
        return shouldSimulateGenomes;
    }

    public static void AdvanceGeneration()
    {
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        
        foreach(GameObject braid in braids)
            Destroy(braid);

        ResetSimulationValues();
        IECManager.SetUIToModellingState(populationSize);
    }

    public static bool HasControllersEvaluated()
    {
        return (evaluationsMade == populationSize);
    }

    public static bool HasControllersCreatedData()
    {
        return (vectorArraysMade == populationSize); 
    }

    public static void ResetSimulationValues()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables();
        vectorArraysMade = 0;
        evaluationsMade = 0;
        shouldSimulateGenomes = true; 
    }
}
