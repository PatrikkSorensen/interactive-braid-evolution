using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class CPPNController : UnitController
{
    public override void Activate(IBlackBox box)
    {
        ISignalArray inputArr = box.InputSignalArray;

        inputArr[0] = 0.1f;
        inputArr[1] = 0.2f;
        inputArr[2] = 0.3f;

        box.Activate();
        ISignalArray outputArr = box.OutputSignalArray;

        Debug.Log(outputArr[0]);
        Debug.Log(outputArr[1]);
        Debug.Log(outputArr[2]);
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
