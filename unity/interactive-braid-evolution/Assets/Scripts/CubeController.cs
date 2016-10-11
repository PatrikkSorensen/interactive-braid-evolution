using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class CubeController : UnitController {


    public override void Activate(IBlackBox box)
    {
        
    }

    public override float GetFitness()
    {
        return 2.0f; 
    }

    public override void Stop()
    {

    }
}
