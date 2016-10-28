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
        //TODO: Clean this up and refactor it into functions for debugging
        neat = box;
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        int i = 0;


        testVectors = new Vector3[20]; 
        for (i = 0; i < 20; i++)
        {
            testVectors[i] = new Vector3(0.0f, 0.0f, i * 2); 
        }

        testVectors = NormalizeInputVectors(testVectors);

        // Set up inputs as array and feed it to the network
        ISignalArray inputArr = neat.InputSignalArray;

        MessageVectors = new Vector3[testVectors.Length];

        i = 0; 
        Debug.Log("******** " + gameObject.name + " is creating braid segments ********");
        foreach (Vector3 v in testVectors)
        {

            inputArr[0] = v.x;
            inputArr[1] = v.y;
            inputArr[2] = v.z;


            neat.Activate();

            ISignalArray outputArr = neat.OutputSignalArray;
            
            MessageVectors[i] = NormalizeOutput(outputArr, i);

            i++; 
            //DebugRawNetwork(inputArr, outputArr);
            
        }
        messenger.AddVectors(braidId - 1, MessageVectors);
        DebugNormalizedNetwork();

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
    public Vector3 NormalizeOutput (ISignalArray outputs, int vectorCounter)
    {
        float min = -10.0f;
        float max = 10.0f;

        double x = outputs[0] * 10;
        double y = outputs[1] * 10;
        //Debug.Log("OUTPUTS: x: " + x + ", y: " + y); 

        Vector3 v = new Vector3((float) x, (float) y, vectorCounter);


        return v;
    }

    private Vector3[] NormalizeInputVectors(Vector3[] vectors)
    {
        float min = -10.0f;
        float max = 10.0f; 

        //Debug.Log("Normalizing inputs");
        Vector3[] normalizedArray = new Vector3[vectors.Length];

        for (int i = 0; i < vectors.Length; i++)
        {
            float x = (vectors[i].x - min) / (max - min) * 2 - 1;
            float y = (vectors[i].y - min) / (max - min) * 2 - 1;
            float z = (vectors[i].z - min) / (max - min) * 2 - 1;
            Vector3 newVect = new Vector3(x, y, z);
            normalizedArray[i] = newVect; 
        }

        return normalizedArray;
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

    public void PrintRawInputs(ISignalArray inputs)
    {
        Debug.Log("******** RAW INPUT ARRAY FOR " + gameObject.name + ": ********");
        for (int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + inputs[i] + "]");
        }
        Debug.Log("*********************************************************");
    }

    public void PrintRawOutputs(ISignalArray outputs)
    {

        Debug.Log("******** RAW OUTPUT ARRAY FOR " + gameObject.name + ": ********");
        for (int i = 0; i < outputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + outputs[i] + "]");
        }

        Debug.Log("*********************************************************");
    }

    public void DebugRawNetwork(ISignalArray inputs, ISignalArray outputs)
    {
            //PrintRawInputs(inputs);
            PrintRawOutputs(outputs);
    }

    private void DebugNormalizedNetwork()
    {
        PrintMessageVectors(); 
    }
}
