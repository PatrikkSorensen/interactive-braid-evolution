using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;
using ExperimentTypes;
using System.Collections.Generic;

public class BraidController : UnitController
{

    // Evolution specific variables 
    public int CURRENT_GENERATION; 
    public int VECTOR_ARRAY_SIZE;
    public int NUM_INPUTS;
    public int NUM_OUTPUTS;

    double[] INPUT_ARRAY;
    double[] OUTPUT_ARRAY;
    double[] DELTA_ARRAY;
    double[] VECTOR_ARRAY;
    double[] RADIUS_ARRAY; 

    protected IBlackBox neat;
    protected float fitness = 0.0f;

    // Message variables 
    protected int braidId; 
    ModelMessager messenger;
    public Vector3[] BraidVectors; 

    // Braid specific variables
    public int BraidId {
        get { return braidId; }
        set { braidId = value; }
    }

    public override void Activate(IBlackBox box)
    {
        neat = box;
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        SetupCPPNController();
        ActivateBraidController(); 
    }

    protected void ActivateBraidController()
    {
        ActivateCPPNSplit(); 
        BraidVectors = UtilityHelper.OutputsToBraidVectors(VECTOR_ARRAY, VECTOR_ARRAY_SIZE);

        messenger.AddRadiusArray(braidId, RADIUS_ARRAY);
        messenger.AddVectors(braidId, BraidVectors);
        BraidSimulationManager.vectorArraysMade++; 
    }
    void ActivateCPPNSplit()
    {
        ISignalArray inputArr = neat.InputSignalArray;

        double layer = -1.0; 
        for (int i = 0; i < INPUT_ARRAY.Length; i++)
        {
            inputArr[0] = layer;
            inputArr[1] = INPUT_ARRAY[i];  

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;
        }
    }


    void ActivateCPPN()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        int i, j;
        for (i = j = 0; i < INPUT_ARRAY.Length; i += 3, j++)
        {
            Vector3 v = new Vector3((float) INPUT_ARRAY[i], (float) INPUT_ARRAY[i + 1], (float) INPUT_ARRAY[i + 2]); 
            double distance = UtilityHelper.GetDistanceFromCenter(v, 10.0f, -10.0f);

            inputArr[0] = INPUT_ARRAY[i];      // x
            inputArr[1] = INPUT_ARRAY[i + 1];  // y
            inputArr[2] = INPUT_ARRAY[i + 2];  // z
            inputArr[3] = distance; 

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            DELTA_ARRAY[i] += outputArr[0]; 
            DELTA_ARRAY[i + 1] += outputArr[1]; 
            DELTA_ARRAY[i + 2] += outputArr[2];
            RADIUS_ARRAY[j] = outputArr[4]; 
        }

        VECTOR_ARRAY = UtilityHelper.MergeArraysFromVectorANN(INPUT_ARRAY, DELTA_ARRAY);
    }

    void SetupCPPNController()
    {
        SetupANNStructure(5, 4, 5);

        INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE, -10, 10); 
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, -10, VECTOR_ARRAY_SIZE * 2);
        RADIUS_ARRAY = new double[VECTOR_ARRAY_SIZE];
    }


    void SetANNVectorArray()
    {
        Vector3[] temp = messenger.GetVectors(BraidId);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(temp);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, -10, VECTOR_ARRAY_SIZE * 2);
    }

    void SetupANNStructure(int vectorSize, int inputSize, int outputSize)
    {
        
        VECTOR_ARRAY_SIZE = vectorSize;
        NUM_INPUTS = inputSize;
        NUM_OUTPUTS = outputSize;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS];
        DELTA_ARRAY = new double[VECTOR_ARRAY_SIZE * 3];
    }

    public void SetFitness(float newFitness) { fitness = newFitness; }

    public override float GetFitness() { return fitness; }

    public override void Stop() { Debug.Log("Stop braidController called"); }
}
