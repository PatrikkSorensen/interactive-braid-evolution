using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class BraidController : UnitController
{
    // Input vectors
    public Vector3[] inputVectors;
    public int VECTOR_ARRAY_SIZE = 3; 

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

    // Debugging variables 
    double[] debugInputArray;
    double[] debugOutputArray; 
    public override void Activate(IBlackBox box)
    {
        neat = box; int i = 0;
        ISignalArray inputArr = neat.InputSignalArray;
        InitializeControllerVariables(); 

        foreach (Vector3 v in inputVectors)
        {
            double input = v.z / 10.0;

            inputArr[2] = input;
            neat.Activate();
            ISignalArray outputArr = neat.OutputSignalArray;

            // debugging
            debugInputArray[i] = input;
            debugOutputArray[i] = outputArr[0];
            debugOutputArray[i + 1] = outputArr[1]; 

            i++; 
        }
        DebugNetwork(); 

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

    public void InitializeControllerVariables()
    {
        Debug.Log("Initializing and activating controller: " + gameObject.name); 
        VECTOR_ARRAY_SIZE = 3; 
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        debugInputArray = new double[VECTOR_ARRAY_SIZE]; // NOTE, inputs are only one value atm
        debugOutputArray = new double[VECTOR_ARRAY_SIZE * 2]; // NOTE: inputs are only two values (x and y atm)
        inputVectors = CreateInputVectors();
    }

    /********************** UTILITY FUNCTIONS FOR NETWORK **********************/
    public Vector3[] CreateInputVectors()
    {
        inputVectors = new Vector3[VECTOR_ARRAY_SIZE];
        for (int i = 0; i < inputVectors.Length; i++)
        {
            inputVectors[i] = new Vector3(0.0f, 0.0f, i * 2);
        }
        return inputVectors;
    }

    /********************** DEBUGGING AND TESTING FUNCTIONS **********************/

    public void DebugNetwork()
    {
        foreach(double input in debugInputArray)
            Debug.Log("i: " + input);
       
        foreach (double d in debugOutputArray)
            Debug.Log("o: " + d);


        Debug.Log(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
    }
}
