using UnityEngine;
using UnityEngine.UI; 
using SharpNeat.EvolutionAlgorithms; 
using System.Collections;
using DG.Tweening; 

public class BraidSelector : MonoBehaviour {

    public static bool ReadyToProgressEvolution;
    public static bool ShouldSimulateGenomes;

    void Awake()
    {
        ShouldSimulateGenomes = true; 
        ReadyToProgressEvolution = false; 
    }

	void Update () {

        if (UIStatusWindow.currentStatus != UIStatusWindow.STATUS.SIMULATING)
            return;

    }

    public static bool ReadyForSelection()
    {
        return ReadyToProgressEvolution; 
    }

    public static void SetReadyForSelection(bool b)
    {
        ReadyToProgressEvolution = b;
    }

    public static bool ShouldEvaluate()
    {
        return ShouldSimulateGenomes;
    }

    public static void SetShouldEvaluate(bool b)
    {
        ShouldSimulateGenomes = b;
    }
}
