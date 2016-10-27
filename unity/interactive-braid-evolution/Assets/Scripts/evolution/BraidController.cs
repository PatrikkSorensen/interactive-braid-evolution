using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class BraidController : UnitController
{
    // Input vectors
    public Vector3[] testVectors;

    private GameObject fitnessObjective;
    private IBlackBox neat;
    private float fitness = 0.0f;

    // Message variables 
    private int braidId; 
    ModelMessager messenger;
    Vector3[] MessageVectors; 

    // Braid specific variables
    public int BraidId
    {
        get
        {
            return braidId;
        }

        set
        {
            braidId = value;
        }
    }

    public override void Activate(IBlackBox box)
    {
        neat = box;
        messenger = GameObject.FindObjectOfType<ModelMessager>(); 
        //TODO: There is no test vectors? Should be 0.0f all the way in the start

        // Set up inputs as array and feed it to the network
        ISignalArray inputArr = neat.InputSignalArray;

        MessageVectors = new Vector3[testVectors.Length];

        int i = 0; 
        foreach(Vector3 v in testVectors)
        {
            Debug.Log("vector:" + v); 

            inputArr[0] = v.x;
            inputArr[1] = v.y;
            inputArr[2] = v.z;


            neat.Activate();

            ISignalArray outputArr = neat.OutputSignalArray;
            MessageVectors[i] = NormalizeToVector(outputArr, i);

            i++; 
            //DebugNetwork(inputArr, outputArr);
        }

        Debug.Log("vector?");
        messenger.AddVectors(braidId - 1, MessageVectors); 
        //PrintMessageVectors();

    }

    public void SetFitness(float newFitness)
    {
        fitness = newFitness;
    }

    public override float GetFitness()
    {
        Debug.Log("Returning fitness: " + fitness + " for gameobject: " + gameObject.name); 
        return fitness;
    }

    public override void Stop()
    {
        Debug.Log("Stop braidController called"); 
    }

    

    /********************* NORMALIZING AND ULITIY FUNCTIONS **********************/
    public Vector3 NormalizeToVector (ISignalArray outputs, int vectorCounter)
    {
        // Note, Rhino has the "up/y" direction on an vectors z axis 
        float mulitplier = 10.0f; 
        double x = outputs[0] * mulitplier;
        double y = outputs[2] * mulitplier;
        double z = vectorCounter;

        return (new Vector3((float) x, (float) y, (float) z)); 
    }

    /********************** DEBUGGING AND TESTING FUNCTIONS **********************/

    public void PrintMessageVectors ()
    {
        Debug.Log("******** BRAID VECTORS FOR " + gameObject.name + ": ********");
        foreach (Vector3 v in MessageVectors)
        {
            Debug.Log("Vector" + v);
        }
        Debug.Log("*********************************************");
    }

    public void PrintInputs(ISignalArray inputs)
    {
        Debug.Log("******** INPUT ARRAY FOR " + gameObject.name + ": ********");
        for (int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + inputs[i] + "]");
        }
        Debug.Log("*********************************************************");
    }

    public void PrintOutputs(ISignalArray outputs)
    {

        Debug.Log("******** OUTPUT ARRAY FOR " + gameObject.name + ": ********");
        for (int i = 0; i < outputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + outputs[i] + "]");
        }

        Debug.Log("*********************************************************");
    }

    public void DebugNetwork(ISignalArray inputs, ISignalArray outputs)
    {
            PrintInputs(inputs);
            PrintOutputs(outputs);
    }
}
