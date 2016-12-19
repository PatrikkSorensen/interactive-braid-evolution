using UnityEngine;
using System.Collections;

public class ExitUIManager : MonoBehaviour {


    public void PerformExitStep()
    {
        FindObjectOfType<StoryboardUtility>().StartStoryBoardStep();
    }
}
