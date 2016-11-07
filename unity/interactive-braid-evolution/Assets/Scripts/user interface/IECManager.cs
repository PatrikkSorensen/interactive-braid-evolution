using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

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

        Debug.Log("Save path: " + Application.persistentDataPath); 
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

        if (Input.GetKeyDown(KeyCode.U))
            IECManager.GetSelectionId(); 
    }

    public static int GetSelectionId()
    {

        if (!GameObject.FindObjectOfType<UISelectionWindow>())
            return 0; 

        string resultString = Regex.Match(UISelectionWindow.current_selected.name, @"\d+").Value;
        return (Int32.Parse(resultString));
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
        Destroy(initializeButton);
        Destroy(dropDown);
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
