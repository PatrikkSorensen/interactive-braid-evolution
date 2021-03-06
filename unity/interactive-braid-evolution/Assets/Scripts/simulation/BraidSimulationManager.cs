﻿using UnityEngine;
using System.Collections;
using System;

public class BraidSimulationManager : MonoBehaviour {

    public static int vectorArraysMade; 
    public static int evaluationsMade; 
    public static int populationSize;
    public static int generation; 

    public static bool shouldSimulateGenomes;

    private void Start()
    {
        shouldSimulateGenomes = true;
        vectorArraysMade = 0;
        evaluationsMade = 0;
        generation = 0; 
    }

    public static void AdvanceGeneration()
    {
        Debug.Log("Advancing generation!"); 

        if(GameObject.FindObjectOfType<UIStatusWindow>() == null)
        {
            Debug.Log("No ui window, so wont perform another run");
            return; 
        }
        GameObject[] braids = GameObject.FindGameObjectsWithTag("Braid");
        string[] braidFiles = new string[populationSize];
        int index = 0;

        foreach (GameObject braid in braids)
        {
            if (braid.GetComponent<MaterialScript>().selected)
                braidFiles[index] = braid.name;
            else
                braidFiles[index] = ""; 

            Destroy(braid);
            index++; 
        }

        StoryboardUtility.SaveGenerationData(braidFiles, generation++); 

        ResetSimulationValues();
        IECManager.SetUIToModellingState(populationSize);
    }

    public static void ResetSimulationValues()
    {
        GameObject.FindObjectOfType<UDPReciever>().ResetVariables();
        vectorArraysMade = 0;
        evaluationsMade = 0;
        shouldSimulateGenomes = true;
    }

    public void SetBraidSimulationFromButton(bool b)
    {
        shouldSimulateGenomes = b;
    }

    public static void SetShouldBraidsEvaluate(bool b)
    {
        shouldSimulateGenomes = b;
    }

    public static bool ShouldBraidsEvaluate()
    {
        return shouldSimulateGenomes;
    }

    public static bool HasControllersEvaluated()
    {
        return (evaluationsMade == populationSize);
    }

    public static bool HasControllersCreatedData()
    {
        return (vectorArraysMade == populationSize); 
    }


}
