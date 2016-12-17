using UnityEngine;
using System.Collections;

public class KeyboardSceneManager : MonoBehaviour {

	public void HideUI()
    {
        IECManager.HideUI();
    }

    public void InitializeANN(int setup)
    {
        UIANNSetupDropdown.SetANNSetup(setup); 
        GameObject.FindObjectOfType<Optimizer>().InitializeEA(); 
    }
}
