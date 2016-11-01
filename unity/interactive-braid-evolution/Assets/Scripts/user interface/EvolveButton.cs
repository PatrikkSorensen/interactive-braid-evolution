using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class EvolveButton : MonoBehaviour {

    public static void DisableAllUI()
    {
        SetEvolveButton(false);
        SetModellingButton(false); 
    }

    public static void SetEvolveButton(bool interacteble)
    {
        Button m_button = GameObject.Find("EvolveButton").GetComponentInChildren<Button>();
        m_button.interactable = interacteble;
    }

    public static void SetModellingButton(bool interacteble)
    {
        Button m_button = GameObject.Find("ModellingButton").GetComponentInChildren<Button>();
        m_button.interactable = interacteble;
    }

}
