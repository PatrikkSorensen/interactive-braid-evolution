using UnityEngine;
using System.Collections;


public class UtilityHelper : MonoBehaviour {

    public static double[] CreateInputVector3Array(int numVectors)
    {
        double[] inputArray = new double[numVectors * 3]; 
        Vector3[] inputVectors = UtilityHelper.CreateRandomVectors(0, 10, numVectors, 2);

        for(int i = 0, j = 0; i < inputVectors.Length; i++, j += 3)
        {
            //NOTE: Be careful if this is the right dimensions
            inputArray[j] = inputVectors[i].x;
            inputArray[j + 1] = inputVectors[i].y;
            inputArray[j + 2] = inputVectors[i].z;
        }

        return inputArray;
    }

    public static double[] NormalizeInputVector3Array(double[] inputs, float min, float max)
    {
        double[] normalizedArray = new double[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            double x = (inputs[i] - min) / (max - min);
            //Debug.Log("input: " + inputs[i] + " became normalized to: " + x); 
            normalizedArray[i] = x;
        }

        return normalizedArray;
    }

    public static Vector3[] CreateRandomVectors(int min, int max, int size, int yOffset)
    {
        Vector3[] v = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            v[i] = new Vector3(Random.Range(min, max), Random.Range(min, max), i * yOffset);
        }

        return v;
    }

    #region double array normalizing and creation
    /// <summary>
    /// Creates a double array with value i * 2, where i is going from 0 to size
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static double[] CreateInputDoubleArray(int size)
    {
        double[] inputVectors = new double[size];
        for (int i = 0; i < inputVectors.Length; i++)
            inputVectors[i] = (double)i * 2;

        return inputVectors;
    }

    /// <summary>
    /// Normalizes values between min and max in a range of [-1, 1]
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public static double[] NormalizeInputDoubleArray(double[] inputs, float min, float max)
    {
        double[] normalizedArray = new double[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            double x = (inputs[i] - min) / (max - min);
            //Debug.Log("input: " + inputs[i] + " became normalized to: " + x); 
            normalizedArray[i] = x;
        }

        return normalizedArray;
    }
    #endregion

    /********************* NORMALIZING AND ULITIY FUNCTIONS **********************/
    public static Vector3[] OutputsToBraidVectors(double[] inputs, double[] outputs, int size)
    {
        Vector3[] braidVectors = new Vector3[size]; 
        //TODO: Make this use the NormalizeHelper class 
        int j = 0;
        for (int i = 0; i < size; i++)
        {
            braidVectors[i] = Vector3.up;

            float x = (float)outputs[j] * 10.0f;
            float y = (float)outputs[j + 1] * 10.0f;
            float z = (float)(inputs[i] * UISliderUpdater.GetValue()); // Has to be made positive
            braidVectors[i] = new Vector3(x, y, z);

            j += 2;
        }

        return braidVectors; 
    }
}
