using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class CubeController : UnitController {

    private IBlackBox neat;
    private bool isRunning;

    void FixedUpdate()
    {

        if (isRunning)
        {
            // Set up inputs as array and feed it to the network
            ISignalArray inputArr = neat.InputSignalArray;
            inputArr[0] = 0.0f;
            inputArr[1] = 0.0f;
            inputArr[2] = 0.0f;
            inputArr[3] = 0.0f;
            inputArr[4] = 0.0f;

            //PrintInputs(inputArr);

            neat.Activate();

            // Get outputs
            ISignalArray outputArr = neat.OutputSignalArray;
            int i = 0; 
           // PrintOutputs(outputArr);
        }

    }

    public override void Activate(IBlackBox box)
    {
        neat = box;
        isRunning = true; 
    }

    public override float GetFitness()
    {
        return 2.0f; 
    }

    public override void Stop()
    {
        isRunning = false; 
    }

    public void PrintInputs(ISignalArray inputs)
    {
        Debug.Log("******** INPUT ARRAY: ********");
        for(int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + inputs[i] + "]"); 
        }
    }

    public void PrintOutputs (ISignalArray outputs)
    {
        for (int i = 0; i < outputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + outputs[i] + "]");
        }
    }
}
