using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IECManager : MonoBehaviour {

    public static GameObject evolveButton;
    public static GameObject selectionWindow;
    public static GameObject slider;
    public static GameObject statusWindowContainer;
    public static GameObject initializeButton;
    public static GameObject dropDown;

    private void Start()
    {
        initializeButton      = GameObject.Find("InitializeANNButton");
        evolveButton          = GameObject.Find("EvolveButton");
        selectionWindow       = GameObject.FindObjectOfType<UISelectionWindow>().gameObject;
        slider                = GameObject.FindObjectOfType<UISliderUpdater>().gameObject;
        statusWindowContainer = GameObject.FindObjectOfType<UIStatusWindow>().gameObject;
        dropDown              = GameObject.Find("ANNSetupDropdown");
        //Debug.Log("Drop:" + dropDown); 

        SetStartUI();
    }

    public static void SetStartUI()
    {
        evolveButton.SetActive(false); 
        selectionWindow.SetActive(false);

        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.UNINITIALIZED); 
    }

    public static void SetUIToEvolvingState()
    {
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.EVOLVING);
        evolveButton.SetActive(true);

        // Destroy?
        initializeButton.SetActive(false);
        dropDown.SetActive(false);
    }

    public static void SetUIToSelectionState ()
    {
        evolveButton.SetActive(false);
        selectionWindow.SetActive(true);
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.SIMULATING);
    }

    internal static void SetUIToModellingState()
    {
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.MODELLING);
    }
}
