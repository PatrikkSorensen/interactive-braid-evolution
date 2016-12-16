using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening; 
using System.Text.RegularExpressions;
using APP_STATUS; 
public class IECManager : MonoBehaviour {

    UDPReciever reciever;

    // ui animator
    private static Animator uiAnim;
    private static Animator exitAnim; 
    // ui components
    public static GameObject evolveButton;
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
        statusWindowContainer = GameObject.FindObjectOfType<UIStatusWindow>().gameObject;
        dropDown              = GameObject.Find("ANNSetupDropdown");
        advanceButton         = GameObject.Find("AdvanceGeneration");
        exitButton            = GameObject.Find("ExitButton");
        loadDropdown          = GameObject.Find("LoadDropDown");
        generationCounter        = GameObject.Find("GenerationCounter"); 
        SetStartUI();

        uiAnim = GameObject.Find("- ui v2").GetComponent<Animator>();
        exitAnim = GameObject.Find("- exit canvas").GetComponent<Animator>(); 
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
        //advanceButton.SetActive(false);
        //evolveButton.SetActive(false); 
        exitButton.SetActive(false);
    }

    public static void SetUIToEvolvingState()
    {
        UIStatusWindow.SetStatus(STATUS.EVOLVING);
        //evolveButton.SetActive(true);

        Destroy(initializeButton);
        Destroy(dropDown);
        Destroy(loadDropdown); 
    }

    internal static void SetUIToModellingState(int populationSize)
    {
        //evolveButton.SetActive(false);
        exitButton.SetActive(true);
        advanceButton.SetActive(false);

        UIStatusWindow.totalModels = populationSize;
        UIStatusWindow.SetStatus(STATUS.MODELLING);
    }

    public static void SetUIToSelectionState()
    {
        evolveButton.SetActive(false);
        advanceButton.SetActive(true);
        exitButton.SetActive(true); 

        UIStatusWindow.SetStatus(STATUS.SIMULATING);

    }

    public static void SetUIToExitState()
    {
        //evolveButton.SetActive(false);
        //advanceButton.SetActive(false);
        //exitButton.SetActive(false);

        UIStatusWindow.SetStatus(STATUS.SIMULATING);
        uiAnim.SetTrigger("advance");
        exitAnim.SetTrigger("advance");
        FindObjectOfType<UserController>().DisableController(); 
    }

    /*********** END OF UI STATES **********/
    public static void HideUI()
    {
        DisableAllButtons(); 
    }

    public static void DisableAllButtons()
    {
        //initializeButton.SetActive(false);
        //dropDown.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
        //dropDown.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
        //dropDown.SetActive(false);
        advanceButton.SetActive(false);
        //evolveButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public static void SetGeneration(uint generation)
    {
        generationCounter.GetComponentInChildren<Text>().text = "Generation: " + generation.ToString(); 
    }
}
