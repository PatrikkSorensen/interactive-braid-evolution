using UnityEngine;
using System.Collections;

public class LeapSceneManager : MonoBehaviour {

	public void StartEA()
    {
        FindObjectOfType<Optimizer>().InitializeEA(); 
    }
}
