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
    double[] DELTA_ARRAY;
    double[] VECTOR_ARRAY;
    double[] MATERIAL_ARRAY;
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
        InitializeBraidControllerVariables(box);
        ActivateBraidController(); 
    }

    protected void ActivateBraidController()
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
                break;
            case ANNSetup.CPPN_BASED:
                ActivateCPPNController();
                break;
            case ANNSetup.CPPN_VER2:
                ActivateCPPNVer2();
                break;
            default:
                break;
        }

        BraidVectors = UtilityHelper.OutputsToBraidVectors(VECTOR_ARRAY, VECTOR_ARRAY_SIZE);

        messenger.AddMaterialArray(braidId, MATERIAL_ARRAY);
        messenger.AddRadiusArray(braidId, RADIUS_ARRAY);
        messenger.AddVectors(braidId, BraidVectors);
        BraidSimulationManager.vectorArraysMade++; 
    }

    protected void ActivateSimpleBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i++)
        {
            inputArr[0] = INPUT_ARRAY[i]; 

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            OUTPUT_ARRAY[i] = outputArr[0] * 2 - 1;
            OUTPUT_ARRAY[i + 1] = outputArr[1] * 2 - 1;
        }

        VECTOR_ARRAY = UtilityHelper.MergeArraysFromSimpleANN(INPUT_ARRAY, OUTPUT_ARRAY); 
    }

    protected void ActivateVectorBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i += 3)
        {
            inputArr[0] = INPUT_ARRAY[i];      // x
            inputArr[1] = INPUT_ARRAY[i + 1];  // y
            inputArr[2] = INPUT_ARRAY[i + 2];  // z

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            DELTA_ARRAY[i]      += outputArr[0] * 2 - 1; // x
            DELTA_ARRAY[i + 1]  += outputArr[1] * 2 - 1; // y
            DELTA_ARRAY[i + 2]  += outputArr[2] * 2 - 1; // z
        }

        VECTOR_ARRAY = UtilityHelper.MergeArraysFromVectorANN(INPUT_ARRAY, DELTA_ARRAY);
    }

    protected void ActivateCPPNController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i += 3)
        {
            inputArr[0] = INPUT_ARRAY[i];      // x
            inputArr[1] = INPUT_ARRAY[i + 1];  // y
            inputArr[2] = INPUT_ARRAY[i + 2];  // z
            //inputArr[3] = INPUT_ARRAY[i + 2];  // distance away from center

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            DELTA_ARRAY[i] += outputArr[0]; // x
            DELTA_ARRAY[i + 1] += outputArr[1]; // y
            DELTA_ARRAY[i + 2] += outputArr[2]; // z

            //DELTA_ARRAY[i + 2] += outputArr[2]; // material output
            //DELTA_ARRAY[i + 2] += outputArr[2]; // radius
        }

        VECTOR_ARRAY = UtilityHelper.MergeArraysFromVectorANN(INPUT_ARRAY, DELTA_ARRAY);

    }

    protected void ActivateCPPNVer2()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        int i, j;
        for (i = j = 0; i < INPUT_ARRAY.Length; i += 3, j++)
        {
            Vector3 v = new Vector3((float) INPUT_ARRAY[i], (float) INPUT_ARRAY[i + 1], (float) INPUT_ARRAY[i + 2]); 
            double distance = UtilityHelper.GetDistanceFromCenter(v, 10.0f, -10.0f);
            //Debug.Log(distance); 

            inputArr[0] = INPUT_ARRAY[i];      // x
            inputArr[1] = INPUT_ARRAY[i + 1];  // y
            inputArr[2] = INPUT_ARRAY[i + 2];  // z
            inputArr[3] = distance; 

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            DELTA_ARRAY[i] += outputArr[0]; 
            DELTA_ARRAY[i + 1] += outputArr[1]; 
            DELTA_ARRAY[i + 2] += outputArr[2];
            MATERIAL_ARRAY[j] = outputArr[3]; 
            RADIUS_ARRAY[j] = outputArr[4]; 
        }

        VECTOR_ARRAY = UtilityHelper.MergeArraysFromVectorANN(INPUT_ARRAY, DELTA_ARRAY);
    }

    protected void ActivateRandomBraidController()
    {
        messenger.SendRandomBraidArrays(VECTOR_ARRAY_SIZE);
    }

    /********************* FUNCTIONS FOR SETTING UP CONTROLLERS **********************/
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
            case ANNSetup.CPPN_BASED:
                SetupCPPNStructure();
                break;
            case ANNSetup.CPPN_VER2:
                SetupCPPNVer2Structure();
                break;
            default:
                Debug.LogError("No braid controller variables has been initialized."); 
                break;
        }
    }

    protected void SetupSimpleANNStructure()
    {
        SetupANNStructure(5, 1, 2);
        INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.NormalizeInputDoubleArray(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // max and min
    }

    protected void SetupVectorANNStructure()
    {
        SetupANNStructure(5, 3, 3); 

        if (Optimizer.Generation < 1)
            InitializeVectorANNStructure();
        else
            SetANNVectorArray();
    }

    protected void SetupRandomANNStructure()
    {
        VECTOR_ARRAY_SIZE = 12;
    }

    protected void SetupCPPNStructure()
    {
        SetupANNStructure(5, 3, 3);

        if (Optimizer.Generation < 1)
            InitializeVectorANNStructure();
        else
            SetANNVectorArray();
    }

    protected void SetupCPPNVer2Structure()
    {
        SetupANNStructure(5, 4, 5); 

        if (Optimizer.Generation < 1) 
            InitializeVectorANNStructure();
        else
            SetANNVectorArray();

        MATERIAL_ARRAY = new double[VECTOR_ARRAY_SIZE];
        RADIUS_ARRAY = new double[VECTOR_ARRAY_SIZE];
    }


    protected void SetANNVectorArray()
    {
        Vector3[] temp = messenger.GetVectors(BraidId);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(temp);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, -10, VECTOR_ARRAY_SIZE * 2);
    }

    protected void InitializeVectorANNStructure()
    {
        Vector3[] TEMP_ARRAY = UtilityHelper.CreateEmptyVector3Array(VECTOR_ARRAY_SIZE, -10, 10);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(TEMP_ARRAY);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, -10, VECTOR_ARRAY_SIZE * 2);
    }


    protected void SetupANNStructure(int vectorSize, int inputSize, int outputSize)
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
