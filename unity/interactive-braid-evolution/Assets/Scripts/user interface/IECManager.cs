using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IECManager : MonoBehaviour {

    UDPReciever reciever; 

    // ui components
    public static GameObject evolveButton;
    public static GameObject selectionWindow;
    public static GameObject slider;
    public static GameObject statusWindowContainer;
    public static GameObject initializeButton;
    public static GameObject dropDown;
    public static GameObject advanceButton; 

    private void Start()
    {
        reciever = GameObject.FindObjectOfType<UDPReciever>(); 

        initializeButton      = GameObject.Find("InitializeANNButton");
        evolveButton          = GameObject.Find("EvolveButton");
        selectionWindow       = GameObject.FindObjectOfType<UISelectionWindow>().gameObject;
        slider                = GameObject.FindObjectOfType<UISliderUpdater>().gameObject;
        statusWindowContainer = GameObject.FindObjectOfType<UIStatusWindow>().gameObject;
        dropDown              = GameObject.Find("ANNSetupDropdown");
        advanceButton         = GameObject.Find("AdvanceGeneration"); 

        SetStartUI();


    }

    private void Update()
    {
        if (reciever.hasImportedAllModels)
        {
            SetUIToSelectionState();
            reciever.hasImportedAllModels = false;
        }
    }

    public static void SetStartUI()
    {
        advanceButton.SetActive(false);
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

    internal static void SetUIToModellingState()
    {
        evolveButton.SetActive(false);
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.MODELLING);
    }

    public static void SetUIToSelectionState()
    {
        evolveButton.SetActive(false);
        selectionWindow.SetActive(true);
        advanceButton.SetActive(true);
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.SIMULATING);
    }
}
