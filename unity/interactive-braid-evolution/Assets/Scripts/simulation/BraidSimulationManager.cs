using UnityEngine;
using System.Collections;

public class BraidSimulationManager : MonoBehaviour {

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

    }

    public static void ResetObjImportVariables()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables(); 
    }
}
