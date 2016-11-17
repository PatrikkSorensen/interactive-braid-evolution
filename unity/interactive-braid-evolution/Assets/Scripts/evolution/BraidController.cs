using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;
using ExperimentTypes; 

public class BraidController : UnitController
{

    // Evolution specific variables 
    public int CURRENT_GENERATION; 
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
    public Vector3[] BraidVectors; 

    // Braid specific variables
    public int BraidId {
        get { return braidId; }
        set { braidId = value; }
    }

    public override void Activate(IBlackBox box)
    {
        InitializeBraidControllerVariables(box);
        ActivateBraidController(); 
    }

    /********************* CONTROLLER SPECIFIC ACTIVATION FUNCTIONS **********************/
    private void ActivateBraidController()
    {
        switch (Optimizer.ANN_SETUP)
        {
            case ANNSetup.SIMPLE:
                ActivateSimpleBraidController();
                break;
            case ANNSetup.VECTOR_BASED:
                ActivateVectorBraidController();
                break;
            case ANNSetup.RANDOM_VECTORS:
                ActivateRandomBraidController();
                return;
            default:
                break;
        }

        BraidVectors = UtilityHelper.OutputsToBraidVectors(INPUT_ARRAY, INPUT_ARRAY, VECTOR_ARRAY_SIZE);
        messenger.AddVectors(braidId, BraidVectors);
    }

    private void ActivateSimpleBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i++)
        {
            inputArr[0] = INPUT_ARRAY[i]; 
            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            OUTPUT_ARRAY[i] = outputArr[0];
            OUTPUT_ARRAY[i + 1] = outputArr[1];
        }
    }

    private void ActivateVectorBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0, j = 0; i < INPUT_ARRAY.Length; i += 3, j += 3)
        {
            //inputArr[0] = INPUT_ARRAY[i];      // x
            //inputArr[1] = INPUT_ARRAY[i + 1];  // y
            //inputArr[2] = INPUT_ARRAY[i + 2];  // z

            //neat.Activate();
            //ISignalArray outputArr = neat.OutputSignalArray;

            //OUTPUT_ARRAY[j]     = outputArr[0]; // x
            //OUTPUT_ARRAY[j + 1] = outputArr[1]; // y
            //OUTPUT_ARRAY[j + 2] = outputArr[2]; // z
        }
    }

    private void ActivateRandomBraidController()
    {
        messenger.SendRandomBraidArrays(VECTOR_ARRAY_SIZE);
    }

    /********************* CONTROLLER SETUP FUNCTIONS **********************/
    public void InitializeBraidControllerVariables(IBlackBox box)
    {
        neat = box;
        messenger = GameObject.FindObjectOfType<ModelMessager>();

        switch (Optimizer.ANN_SETUP)
        {
            case ANNSetup.SIMPLE:
                SetupSimpleANNStructure();
                break;
            case ANNSetup.VECTOR_BASED:
                SetupVectorANNStructure();
                break;
            case ANNSetup.RANDOM_VECTORS:
                SetupRandomANNStructure();
                break;
            default:
                break;
        }
    }

    private void SetupSimpleANNStructure()
    {
        SetupANNStructure(5, 1, 2);
        INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.NormalizeInputDoubleArray(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // max and min
    }

    private void SetupVectorANNStructure()
    {
        SetupANNStructure(5, 3, 3); 

        if (Optimizer.Generation < 1)
            InitializeVectorANNStructure();
        else
            SetANNVectorArray();
    }

    private void SetANNVectorArray()
    {
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(messenger.GetVectors(BraidId));
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); 
    }

    private void InitializeVectorANNStructure()
    {
        //Vector3[] TEMP_ARRAY = UtilityHelper.CreateEmptyVector3Array(VECTOR_ARRAY_SIZE);
        Vector3[] TEMP_ARRAY = UtilityHelper.CreateRandomVectors(0, 10, VECTOR_ARRAY_SIZE, 2);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(TEMP_ARRAY);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2);

        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i += 3)
        {
            //inputArr[0] = INPUT_ARRAY[i];      // x
            //inputArr[1] = INPUT_ARRAY[i + 1];  // y
            //inputArr[2] = INPUT_ARRAY[i + 2];  // z

            //neat.Activate();
            //ISignalArray outputArr = neat.OutputSignalArray;

            //INPUT_ARRAY[i] += outputArr[0];
            //INPUT_ARRAY[i + 1] += outputArr[1];
            //INPUT_ARRAY[i + 2] += outputArr[2];
        }

        TEMP_ARRAY = UtilityHelper.VectorsToBraidVectors(INPUT_ARRAY, VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(TEMP_ARRAY);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2);
    }

    private void SetupANNStructure(int vectorSize, int inputSize, int outputSize)
    {
        VECTOR_ARRAY_SIZE = vectorSize;
        NUM_INPUTS = inputSize;
        NUM_OUTPUTS = outputSize;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS];
    }

    private void SetupRandomANNStructure()
    {
        VECTOR_ARRAY_SIZE = 12;
    }

    public void SetFitness(float newFitness) { fitness = newFitness; }
    public override float GetFitness() { return fitness; }
    public override void Stop() { Debug.Log("Stop braidController called"); }
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
