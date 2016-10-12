using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;

public class CubeController : UnitController {

    private GameObject fitnessObjective; 
    private IBlackBox neat;
    private bool isRunning;
    private Rigidbody rb;

    // Utility variables: 
    double max, min;

    // Debugging variables
    private float time = 0.0f;
    private bool hasPrinted = false; 

    void Start()
    {

        max = 100;
        min = 0.0f;

        rb = GetComponent<Rigidbody>(); 
        fitnessObjective = GameObject.Find("FitnessObjective");
        if (!fitnessObjective)
            Debug.LogWarning("No fitness objective found in scene."); 
    }
    void FixedUpdate()
    {
        if (isRunning)
        {
            // Set up inputs as array and feed it to the network
            ISignalArray inputArr = neat.InputSignalArray;

            double x = NormalizeValues(max, min, transform.position.x);
            double z = NormalizeValues(max, min, transform.position.z);
            inputArr[0] = x;
            inputArr[1] = z;

            neat.Activate();

            // Get outputs
            //TODO: Make outputs be able to have negative values as well
            ISignalArray outputArr = neat.OutputSignalArray;
            outputArr[0] = (outputArr[0] < 0.5f) ? outputArr[0] * -1 : outputArr[0];
            outputArr[1] = (outputArr[1] < 0.5f) ? outputArr[0] * -1 : outputArr[1];

            float outputX = (float)outputArr[0] * 10.0f;
            float outputZ = (float)outputArr[1] * 10.0f;
            //Debug.Log("Out X: " + outputX + "\nOut Z: " + outputZ);
            rb.AddForce(new Vector3( outputX, 0.0f, outputZ)); 

            //DebugNetwork(inputArr, outputArr); 
        }

        //Vector3 DebugVector = Vector3.zero;
        //DebugVector.x = 0.5f * 20.0f;
        //rb.AddForce(DebugVector);

    }

    //TODO: Make the normalized values be in range of -1 and 1
    public double NormalizeValues (double max, double min, double value)
    {
        double newVal = 0.0f; 
        //Debug.Log("Normalizing values with params: min[" + min + "], max[" + max + "], val[" + value + "]");

        newVal = (value - min) / (max - min); 

        //Debug.Log("Final result: " + newVal); 
        return newVal;
    }

    public void DebugNetwork(ISignalArray inputs, ISignalArray outputs)
    {
        // For debugging
        if (time + 1.0f < Time.time && !hasPrinted)
        {
            PrintInputs(inputs);
            PrintOutputs(outputs);
            hasPrinted = true;
            time = Time.time;

            //Debug.Log((time + 1.0f) + " < " + Time.time + ", hasPrinted:" + hasPrinted);
        }

        if (hasPrinted)
            hasPrinted = !hasPrinted;
    }

    public override void Activate(IBlackBox box)
    {
        neat = box;
        isRunning = true; 
    }

    // TODO: Fitness only awards from moving away from the objective right now!
    public override float GetFitness()
    {
        float fitness = 0.0f; 
        if (fitnessObjective)
        {
            fitness = Vector3.Distance(transform.position, fitnessObjective.transform.position);
            Debug.Log("Fitness: " + fitness);

            return fitness; 
        }
        else
        {
            Debug.LogWarning("No fitness objective specified."); 
        }

        return 0.0f;
    }

    public override void Stop()
    {
        isRunning = false; 
    }

    // DEBUGGING FUNCTIONS: 
    public void PrintInputs(ISignalArray inputs)
    {
        Debug.Log("******** INPUT ARRAY FOR " + gameObject.name + ": ********");
        for(int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + inputs[i] + "]"); 
        }
    }

    public void PrintOutputs (ISignalArray outputs)
    {

        Debug.Log("******** OUTPUT ARRAY FOR " + gameObject.name + ": ********");
        for (int i = 0; i < outputs.Length; i++)
        {
            Debug.Log("[" + i + "]" + " : [" + outputs[i] + "]");
        }
    }
}
