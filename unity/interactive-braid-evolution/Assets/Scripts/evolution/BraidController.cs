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

        //ISignalArray inputArr = neat.InputSignalArray;
        //for(int i = 0; i < INPUT_ARRAY.Length; i++)
        //{
        //    double input = INPUT_ARRAY[i];

        //    inputArr[0] = input;
        //    neat.Activate();
        //    ISignalArray outputArr = neat.OutputSignalArray;

        //    OUTPUT_ARRAY[i] = Math.Round(outputArr[0], 2);
        //    OUTPUT_ARRAY[i + 1] = Math.Round(outputArr[1], 2);
        //}


        //DebugNetwork();
        //PrintBraidVectors();

        //Debug.Log(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");

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
                SetupVectorANNStructure();
                break;
            case ANNSetup.RANDOM_VECTORS:
                ActivateRandomBraidController();
                break;
            default:
                break;
        }
    }
    private void ActivateRandomBraidController()
    {
        //TODO: Too hardcoded, function should be called in optimizer?
            messenger.SendRandomBraidArrays(9);
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

        OutputsToBraidVectors();
        messenger.AddVectors(braidId - 1, BraidVectors);
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
        // hardcoded for now
        VECTOR_ARRAY_SIZE = 5;
        NUM_INPUTS = 1;
        NUM_OUTPUTS = 2;
        OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS]; // NOTE: outputs are only two values (x and y atm)

        INPUT_ARRAY = CreateInputDoubles();
        INPUT_ARRAY = NormalizeHelper.NormalizeInputDoubles(INPUT_ARRAY, 0.0f, VECTOR_ARRAY_SIZE * 2); // max and min
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

    private void SetupRandomANNStructure()
    {
        VECTOR_ARRAY_SIZE = 12;
    }

    /********************* NORMALIZING AND ULITIY FUNCTIONS **********************/
    private void OutputsToBraidVectors()
    {
        //TODO: Make this use the NormalizeHelper class 
        int j = 0;
        for (int i = 0; i < VECTOR_ARRAY_SIZE; i++)
        {
            BraidVectors[i] = Vector3.up;

            float x = (float)OUTPUT_ARRAY[j] * 10.0f;
            float y = (float)OUTPUT_ARRAY[j + 1] * 10.0f;
            float z = (float)(INPUT_ARRAY[i] * UISliderUpdater.GetValue()); // Has to be made positive
            BraidVectors[i] = new Vector3(x, y, z);

            j += 2;
        }

    }

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
