using UnityEngine;
using System.Collections;

public class BraidSimulationManager : MonoBehaviour {

    public static int evaluationsMade; 
    public static int populationSize; 

    public void SetFlagsFromButton(bool b)
    {
        BraidSelector.SetReadyForSelection(b);
    }
    
    public static bool HasControllersEvaluated()
    {
        bool b = (evaluationsMade == populationSize);
        Debug.Log("Returning some bool: " + b);
        return b; 
    }

	public static void AdvanceGeneration()
    {
        // the actual models
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        
        foreach(GameObject braid in braids)
            Destroy(braid);

        // the ann controllers
        BraidSelector.SetShouldEvaluate(false);
        BraidSelector.SetReadyForSelection(false);

        // variable resetting
        ResetObjImportVariables();

        Debug.Log("Simulation reset and cleaned up, ready for the next one");
        IECManager.SetUIToModellingState();
    }

    public static void ResetObjImportVariables()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables(); 
    }
}
