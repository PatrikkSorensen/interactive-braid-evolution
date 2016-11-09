﻿using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;
using ExperimentTypes; 

public class BraidController : UnitController
{

    // Evolution specific variables 
    public int CURRENT_GENERATION; 

    // Input vectors
    //public Vector3[] inputVectors;
    //public double[] inputDoubles; 
    public int VECTOR_ARRAY_SIZE;
    public int NUM_INPUTS;
    public int NUM_OUTPUTS;
    double[] INPUT_ARRAY;
    double[] OUTPUT_ARRAY;

    private IBlackBox neat;
    private float fitness = 0.0f;

    // Message variables 
    private int braidId; 
    ModelMessager messenger;
    public Vector3[] BraidVectors; // For debugging

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
    public ANNSetup networkSetup; 

    // Debugging variables 


    public void InitializeControllerVariables()
    {

        switch (networkSetup)
        {
            case ANNSetup.SIMPLE:
                SetupSimpleANNStructure();
                break;
            case ANNSetup.VECTOR_BASED:
                SetupVectorANNStructure();
                break;
            default:
                break;
        }
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        BraidVectors = new Vector3[VECTOR_ARRAY_SIZE];



    }

    private void SetupVectorANNStructure()
    {
        Debug.Log("I should do code specfic to the vector based Ann sturcure here..."); 
        // hardcoded for now
        VECTOR_ARRAY_SIZE = 12;
        NUM_INPUTS = 3;
        NUM_OUTPUTS = 3;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS]; // NOTE: outputs are only two values (x and y atm)

        INPUT_ARRAY = CreateInputDoubles();
        INPUT_ARRAY = NormalizeHelper.NormalizeInputDoubles(INPUT_ARRAY, 0.0f, 22.0f); // max and min
    }

    private void SetupSimpleANNStructure()
    {
        // hardcoded for now
        VECTOR_ARRAY_SIZE = 12;
        NUM_INPUTS = 1;
        NUM_OUTPUTS = 2;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS]; // NOTE: outputs are only two values (x and y atm)

        INPUT_ARRAY = CreateInputDoubles();
        INPUT_ARRAY = NormalizeHelper.NormalizeInputDoubles(INPUT_ARRAY, 0.0f, 22.0f); // max and min
    }

    public override void Activate(IBlackBox box)
    {
        neat = box; int i = 0;
        ISignalArray inputArr = neat.InputSignalArray;
        InitializeControllerVariables();

        //Debug.Log("The amount of inputs is: " + inputDoubles.Length);
        foreach (double d in INPUT_ARRAY)
        {
            double input = d;

            inputArr[2] = input;
            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            // debugging
            OUTPUT_ARRAY[i] = Math.Round(outputArr[0], 2);
            OUTPUT_ARRAY[i + 1] = Math.Round(outputArr[1], 2);

            i++; 
        }

        OutputsToBraidVectors();
        messenger.AddVectors(braidId - 1, BraidVectors);
        //DebugNetwork();
        //PrintBraidVectors();

        //Debug.Log(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");

    }

    private void OutputsToBraidVectors()
    {
        //TODO: Make this use the NormalizeHelper class 
        int j = 0; 
        for(int i = 0; i < VECTOR_ARRAY_SIZE; i++)
        {
            BraidVectors[i] = Vector3.up;

            float x = (float) OUTPUT_ARRAY[j] * 10.0f;
            float y = (float) OUTPUT_ARRAY[j + 1] * 10.0f;
            float z = (float) ((INPUT_ARRAY[i] + 1) * 20.0f); // Has to be made positive
            BraidVectors[i] = new Vector3(x, y, z);

            j += 2; 
        }

    }

    public void SetFitness(float newFitness)
    {
        //Debug.Log("Setting fitness to: " + newFitness); 
        fitness = newFitness;
    }

    public override float GetFitness()
    {
        //Debug.Log("Returning fitness: " + fitness + " for gameobject: " + gameObject.name); 
        return fitness;
    }

    public override void Stop()
    {
        Debug.Log("Stop braidController called"); 
    }

    

    /********************* NORMALIZING AND ULITIY FUNCTIONS **********************/
    public Vector3 NormalizeOutput (ISignalArray outputs, int vectorCounter)
    {
        double x = outputs[0] * 10;
        double y = outputs[1] * 10;
        return new Vector3((float)x, (float)y, vectorCounter); 
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

    /********************** UTILITY FUNCTIONS FOR NETWORK **********************/
    //public Vector3[] CreateInputVectors()
    //{
    //    inputVectors = new Vector3[VECTOR_ARRAY_SIZE];
    //    for (int i = 0; i < inputVectors.Length; i++)
    //        inputVectors[i] = new Vector3(0.0f, 0.0f, i * 2);

    //    return inputVectors;
    //}

    public double[] CreateInputDoubles()
    {
        double[] inputVectors = new double[VECTOR_ARRAY_SIZE];
        for (int i = 0; i < inputVectors.Length; i++)
            inputVectors[i] = (double) i * 2;

        return inputVectors;
    }

    /********************** DEBUGGING AND TESTING FUNCTIONS **********************/

    public void DebugNetwork()
    {
        Debug.Log("Debugging network for: " + gameObject.name); 
        foreach(double input in INPUT_ARRAY)
            Debug.Log("i: " + input);
       
        //foreach (double d in outputArray)
        //    Debug.Log("o: " + d);

        for(int i = 0; i < OUTPUT_ARRAY.Length; i+=2)
            Debug.Log("OUTPUT [" + OUTPUT_ARRAY[i] + ", " + OUTPUT_ARRAY[i + 1] + "]");

        Debug.Log(" - - - - - - - - Finished debugging - - - - - - - - "); 
    }

    public void PrintBraidVectors()
    {
        foreach(Vector3 v in BraidVectors)
            Debug.Log(v);
    }
}
