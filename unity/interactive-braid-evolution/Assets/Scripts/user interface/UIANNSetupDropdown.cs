using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using ExperimentTypes; 

public class UIANNSetupDropdown : MonoBehaviour {

    private ANNSetup currentNetworkSetup;
    private Dropdown dropdown;

    public static ANNSetup GetANNSetup()
    {
        Dropdown dropdown = GameObject.Find("ANNSetupDropdown").GetComponent<Dropdown>();
        ANNSetup currentNetworkSetup = (ANNSetup)dropdown.value;
        return currentNetworkSetup;
    }
}

namespace ExperimentTypes
{
    public enum ANNSetup
    {
        SIMPLE,
        VECTOR_BASED,
        MATERIAL_AND_VECTOR, 
        RANDOM_VECTORS
    }
}
