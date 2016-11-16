using UnityEngine;
using System.Collections;

public class BraidSimulationManager : MonoBehaviour {

    public static int evaluationsMade; 
    public static int populationSize; 
    

    public void SetFlagsFromButton(bool b)
    {
        BraidSelector.SetShouldEvaluate(b);
    }
    
    public static bool HasControllersEvaluated()
    {
        //Debug.Log(evaluationsMade + ", " + populationSize); 
        return (evaluationsMade == populationSize); 
    }

	public static void AdvanceGeneration()
    {
        // the actual models
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        
        foreach(GameObject braid in braids)
            Destroy(braid);

        // the ann controllers
        BraidSelector.SetShouldEvaluate(true);
        //BraidSelector.SetReadyToProgressEvolution(false);

        // variable resetting
        ResetObjImportVariables();
        IECManager.SetUIToModellingState(populationSize);

        Debug.Log("Simulation reset and cleaned up, ready for the next one");
    }

    public static void ResetObjImportVariables()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables(); 
    }
}
