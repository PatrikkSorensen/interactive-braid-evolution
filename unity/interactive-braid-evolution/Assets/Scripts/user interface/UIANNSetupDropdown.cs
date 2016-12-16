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

    public static void SetANNSetup(int val)
    {
        //Dropdown dropdown = GameObject.Find("ANNSetupDropdown").GetComponent<Dropdown>();
        //dropdown.value = val;
    }
}

namespace ExperimentTypes
{
    public enum ANNSetup
    {
        CPPN 
    }
}
