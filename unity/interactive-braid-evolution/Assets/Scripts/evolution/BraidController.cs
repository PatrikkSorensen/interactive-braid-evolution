using UnityEngine;
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

    // Debugging variables 

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
        messenger.AddVectors(braidId - 1, BraidVectors);
    }

    private void ActivateVectorBraidController()
    {
        ISignalArray inputArr = neat.InputSignalArray;
        for (int i = 0, j = 0; i < INPUT_ARRAY.Length; i++, j++)
        {
            double inputX = INPUT_ARRAY[i];
            double inputY = INPUT_ARRAY[++i];
            double inputZ = INPUT_ARRAY[++i];
             
            inputArr[0] = inputX;
            inputArr[0] = inputY;
            inputArr[0] = inputZ;

            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            OUTPUT_ARRAY[j] = Math.Round(outputArr[0], 2);
            OUTPUT_ARRAY[++j] = Math.Round(outputArr[1], 2);
            OUTPUT_ARRAY[++j] = Math.Round(outputArr[2], 2);
        }

        //OutputsToBraidVectors();
        messenger.AddVectors(braidId - 1, BraidVectors);
    }

    private void ActivateRandomBraidController()
    {
        //TODO: Too hardcoded, function should be called in optimizer?
        messenger.SendRandomBraidArrays(9);
    }

    /********************* CONTROLLER SETUP FUNCTIONS **********************/
    public void InitializeBraidControllerVariables(IBlackBox box)
    {
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
        neat = box; 
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        BraidVectors = new Vector3[VECTOR_ARRAY_SIZE];
    }


    private void SetupSimpleANNStructure()
    {

        VECTOR_ARRAY_SIZE = 5;
        NUM_INPUTS = 1;
        NUM_OUTPUTS = 2;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS]; 

        INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE);
        // hardcoded for now
        INPUT_ARRAY = UtilityHelper.NormalizeInputDoubleArray(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // max and min
    }

    private void SetupVectorANNStructure()
    {
        VECTOR_ARRAY_SIZE = 12;
        NUM_INPUTS = 3;
        NUM_OUTPUTS = 3;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS];

        INPUT_ARRAY = UtilityHelper.CreateInputVector3Array(VECTOR_ARRAY_SIZE);
        INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, 0.0f, 22.0f); // TODO: Make min and max generic
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
