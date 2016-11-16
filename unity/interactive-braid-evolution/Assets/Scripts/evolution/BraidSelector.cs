using UnityEngine;
using UnityEngine.UI; 
using SharpNeat.EvolutionAlgorithms; 
using System.Collections;
using DG.Tweening; 

public class BraidSelector : MonoBehaviour {

    private static bool ReadyToProgressEvolution;
    private static bool ShouldSimulateGenomes;

    void Awake()
    {
        ShouldSimulateGenomes = true; 
        ReadyToProgressEvolution = false; 
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
