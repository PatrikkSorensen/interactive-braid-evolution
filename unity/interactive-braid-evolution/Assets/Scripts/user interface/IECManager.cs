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
    public static GameObject exitButton;
    public static GameObject loadDropdown;
    public static GameObject generationCounter;

    private void Start()
    {
        reciever = GameObject.FindObjectOfType<UDPReciever>(); 

        initializeButton      = GameObject.Find("InitializeANNButton");
        evolveButton          = GameObject.Find("EvolveButton");
        selectionWindow       = GameObject.Find("SelectionWindow"); 
        slider                = GameObject.FindObjectOfType<UISliderUpdater>().gameObject;
        statusWindowContainer = GameObject.FindObjectOfType<UIStatusWindow>().gameObject;
        dropDown              = GameObject.Find("ANNSetupDropdown");
        advanceButton         = GameObject.Find("AdvanceGeneration");
        exitButton            = GameObject.Find("ExitButton");
        loadDropdown          = GameObject.Find("LoadDropDown");
        generationCounter        = GameObject.Find("GenerationCounter"); 
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

    /*********** UI STATES **********/
    public static void SetStartUI()
    {
        advanceButton.SetActive(false);
        evolveButton.SetActive(false); 
        selectionWindow.SetActive(false);
        exitButton.SetActive(false);
        generationCounter.SetActive(false); 
    }

    public static void SetUIToEvolvingState()
    {
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.EVOLVING);
        evolveButton.SetActive(true);
        generationCounter.SetActive(true); 

        Destroy(initializeButton);
        Destroy(dropDown);
        Destroy(loadDropdown); 
    }

    internal static void SetUIToModellingState(int populationSize)
    {
        evolveButton.SetActive(false);
        exitButton.SetActive(false);
        selectionWindow.SetActive(false);
        advanceButton.SetActive(false);

        UIStatusWindow.totalModels = populationSize;
        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.MODELLING);
    }

    public static void SetUIToSelectionState()
    {
        evolveButton.SetActive(false);
        selectionWindow.SetActive(true);
        advanceButton.SetActive(true);
        exitButton.SetActive(true); 

        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.SIMULATING);

    }

    public static void SetUIToExitState()
    {
        evolveButton.SetActive(false);
        selectionWindow.SetActive(false);
        advanceButton.SetActive(false);
        exitButton.SetActive(false);

        UIStatusWindow.SetStatus(UIStatusWindow.STATUS.SIMULATING);
    }

    /*********** END OF UI STATES **********/

    public static void SetGeneration(uint generation)
    {
        generationCounter.GetComponentInChildren<Text>().text = "Generation: " + generation.ToString(); 
    }



    public static int GetSelectionId()
    {

        if (!GameObject.FindObjectOfType<UISelectionWindow>() || !UISelectionWindow.current_selected)
        {
            Debug.LogError("You tried to get a braid id of a destroyed object"); 
            return 0;
        }

        string resultString = Regex.Match(UISelectionWindow.current_selected.name, @"\d+").Value;
        return (Int32.Parse(resultString));
    }
}
