using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class UIANNSetupDropdown : MonoBehaviour {

    private ANNSetup currentNetworkSetup;
    private Dropdown dropdown;
     
    public enum ANNSetup
    {
        SIMPLE, 
        VECTOR_BASED, 
        MATERIAL_AND_VECTOR
    }

    public static ANNSetup GetANNSetup()
    {
        Dropdown dropdown = GameObject.FindObjectOfType<Dropdown>();
        ANNSetup currentNetworkSetup = (ANNSetup)dropdown.value;
        return currentNetworkSetup;
    }
}
