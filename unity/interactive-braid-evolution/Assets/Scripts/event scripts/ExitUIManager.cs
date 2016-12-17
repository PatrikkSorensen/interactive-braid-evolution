using UnityEngine;
using System.Collections;

public class ExitUIManager : MonoBehaviour {


    public void PerformExitStep()
    {
        Debug.Log("Performing exit loop");
        FindObjectOfType<StoryboardUtility>().StartStoryBoardStep();
    }
}
