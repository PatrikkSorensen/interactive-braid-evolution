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
                break;
            default:
                break;
        }
    }

    private void ActivateSimpleBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0; i < INPUT_ARRAY.Length; i++)
        {
            double input = INPUT_ARRAY[i];

            inputArr[0] = input;
            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            OUTPUT_ARRAY[i] = Math.Round(outputArr[0], 2);
            OUTPUT_ARRAY[i + 1] = Math.Round(outputArr[1], 2);
        }

        BraidVectors = UtilityHelper.OutputsToBraidVectors(INPUT_ARRAY, OUTPUT_ARRAY, VECTOR_ARRAY_SIZE);
        messenger.AddVectors(braidId, BraidVectors);
    }

    private void ActivateVectorBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0, j = 0; i < INPUT_ARRAY.Length; i += 3, j += 3)
        {
            double inputX = INPUT_ARRAY[i];
            double inputY = INPUT_ARRAY[i + 1];
            double inputZ = INPUT_ARRAY[i + 2];
             
            inputArr[0] = inputX;
            inputArr[1] = inputY;
            inputArr[2] = inputZ;

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            double outputX = outputArr[0];
            double outputY = outputArr[1];
            double outputZ = outputArr[2];

            OUTPUT_ARRAY[j] = outputX;
            OUTPUT_ARRAY[j + 1] = outputY;
            OUTPUT_ARRAY[j + 2] = outputZ;
        }

        BraidVectors = UtilityHelper.OutputsToBraidVectors(INPUT_ARRAY, OUTPUT_ARRAY, VECTOR_ARRAY_SIZE);
        messenger.AddVectors(braidId, BraidVectors);
    }

    private void ActivateRandomBraidController()
    {
        messenger.SendRandomBraidArrays(9);
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
        VECTOR_ARRAY_SIZE = 5;
        NUM_INPUTS = 1;
        NUM_OUTPUTS = 2;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS]; 

        INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.NormalizeInputDoubleArray(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // max and min
    }

    private void SetupVectorANNStructure()
    {
        VECTOR_ARRAY_SIZE = 5;
        NUM_INPUTS = 3;
        NUM_OUTPUTS = 3;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS];

        if (Optimizer.Generation < 1)
            InitializeVectorANNStructure();
        else
            SetANNVectorArray();

        
    }

    private void SetANNVectorArray()
    {
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(messenger.GetVectors(BraidId));
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // TODO: Make min and max generic
    }

    private void InitializeVectorANNStructure()
    {
        //Vector3[] TEMP_ARRAY = UtilityHelper.CreateEmptyVector3Array(VECTOR_ARRAY_SIZE);
        Vector3[] TEMP_ARRAY = UtilityHelper.CreateRandomVectors(0, 10, VECTOR_ARRAY_SIZE, 2);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(TEMP_ARRAY);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2);

        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0, j = 0; i < INPUT_ARRAY.Length; i += 3, j+= 3)
        {
            double inputX = INPUT_ARRAY[i];
            double inputY = INPUT_ARRAY[i + 1];
            double inputZ = INPUT_ARRAY[i + 2];

            inputArr[0] = inputX;
            inputArr[1] = inputY;
            inputArr[2] = inputZ;

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            OUTPUT_ARRAY[j] = Math.Round(outputArr[0], 2);
            INPUT_ARRAY[j] = OUTPUT_ARRAY[j];

            OUTPUT_ARRAY[j + 1] = Math.Round(outputArr[1], 2);
            INPUT_ARRAY[j + 1] = OUTPUT_ARRAY[j + 1];

            OUTPUT_ARRAY[j + 2] = Math.Round(outputArr[2], 2);
            INPUT_ARRAY[j + 2] = OUTPUT_ARRAY[j + 2];
        }

        TEMP_ARRAY = UtilityHelper.VectorsToBraidVectors(INPUT_ARRAY, VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.Vector3ToDoubleArray(TEMP_ARRAY);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2);
    }

    private void SetupRandomANNStructure()
    {
        VECTOR_ARRAY_SIZE = 12;
    }

    #region interface functions
    public void SetFitness(float newFitness) { fitness = newFitness; }
    public override float GetFitness() { return fitness; }
    public override void Stop() { Debug.Log("Stop braidController called"); }
    #endregion

    #region debugging functions
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
    #endregion
}
