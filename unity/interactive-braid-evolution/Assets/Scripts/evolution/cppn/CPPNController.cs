using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class CPPNController : UnitController
{
    public override void Activate(IBlackBox box)
    {
        Debug.Log("Activated CPPN Controller");
    }
    public override float GetFitness()
    {
        return 0.0f;
    }

    public override void Stop()
    {
        Debug.Log("Stopping CPPN Controller");
    }
}
