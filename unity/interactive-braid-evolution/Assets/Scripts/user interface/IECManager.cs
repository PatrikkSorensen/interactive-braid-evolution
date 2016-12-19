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
    public Animator uiAnim;
    public Animator exitAnim;
    private static Animator m_uiAnim;
    private static Animator m_exitAnim;

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

        if (uiAnim)
            m_uiAnim = uiAnim;

        if (exitAnim)
            m_exitAnim = exitAnim; 
    }

    ///*********** UI STATES **********/

    public static void SetUIToEvolvingState()
    {
        UIStatusWindow.SetStatus(STATUS.EVOLVING);

        Destroy(initializeButton);
        Destroy(dropDown);
        Destroy(loadDropdown);
    }

    internal static void SetUIToModellingState(int populationSize)
    {
        if(exitButton)
            exitButton.SetActive(true);

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
        // Destroy ui
        if (FindObjectOfType<UIStatusWindow>())
        {
            Destroy(FindObjectOfType<UIStatusWindow>().gameObject); //.SetActive(false);
            Destroy(exitButton);
        }

        // Set ui animations
        m_uiAnim.SetTrigger("advance");

        if(GameObject.Find("LeapEventSystem"))
            m_exitAnim.SetTrigger("leap_advance");
        else
            m_exitAnim.SetTrigger("advance");

        // disable user movement and move him to origin
        GameObject user = GameObject.Find("User");
        if(user.GetComponent<UserController>())
            FindObjectOfType<UserController>().DisableController();

        user.transform.DOMove(Vector3.zero, 4.0f);
        Camera.main.transform.DORotate(Vector3.zero, 4.0f);

        // destroy braids 
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        foreach(GameObject braid in braids)
            Destroy(braid); 
    }

    /*********** END OF UI STATES **********/
    public static void HideUI()
    {
        DisableAllButtons(); 
    }

    public static void DisableAllButtons()
    {
        //advanceButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public static void SetGeneration(uint generation)
    {
        generationCounter.GetComponentInChildren<Text>().text = "Generation: " + generation.ToString(); 
    }
}
