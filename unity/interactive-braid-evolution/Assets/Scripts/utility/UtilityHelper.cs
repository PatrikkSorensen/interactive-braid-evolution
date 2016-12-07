using UnityEngine;
using System.Collections;
using ExperimentTypes; 

public class UtilityHelper : MonoBehaviour {

    public static double[] NormalizeInputVector3Array(double[] inputs, float min, float max)
    {
        double[] normalizedArray = new double[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            double x = inputs[i];
            x += Mathf.Abs(min); 
            x /= (max - min);
            x *= 2;
            x -= 1; 

            normalizedArray[i] = x ;
        }

        return normalizedArray;
    }

    public static double NormalizeDouble(double d, float max, float min)
    {
        d += Mathf.Abs(min);
        d /= (max - min);
        return d; 
    }

    public static Vector3[] CreateRandomVectors(int min, int max, int size, int yOffset)
    {
        int multiplier = (max + Mathf.Abs(min)) / size; 
        Vector3[] v = new Vector3[size];

        for (int i = 0, yValue = min; i < size; i++, yValue += multiplier)
            v[i] = new Vector3(Random.Range(min, max), yValue + multiplier, Random.Range(min, max));

        return v;
    }

    public static Vector3[] CreateEmptyVector3Array(int size, int min, int max)
    {
        int multiplier = (max + Mathf.Abs(min)) / (size - 1);
        Vector3[] v = new Vector3[size];
        int yValue = min - multiplier;

        for (int i = 0; i < size; i++, yValue += multiplier)
            v[i] = new Vector3(0.0f, yValue + multiplier, 0.0f);

        return v;
    }

    public static double[] CreateInputDoubleArray(int size, int min, int max)
    {
        int multiplier = (max + Mathf.Abs(min)) / (size - 1);
        double[] doubles = new double[size];
        int val = min - multiplier;

        for (int i = 0; i < size; i++, val += multiplier)
            doubles[i] = val + multiplier;

        return doubles;
    }

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

    public static Vector3[] OutputsToBraidVectors(double[] outputs, int size)
    {
        Vector3[] braidVectors = new Vector3[size];
        braidVectors = VectorsToBraidVectors(outputs, size);
        return braidVectors;
    }

    public static double GetDistanceFromCenter(Vector3 v, float max, float min)
    {
        //TODO: This is very hardcoded, could be moved as the fourth dimension when creating vector array
        double f = v.magnitude * 10;
        f = NormalizeDouble(f, 17.32f, 0.0f);
        return f; 
    }

    public static Vector3[] DoubleToBraidVectors(double[] inputs, double[] outputs, int size)
    {
        Vector3[] braidVectors = new Vector3[size];
        int j = 0;
        for (int i = 0; i < size; i++)
        {
            braidVectors[i] = Vector3.up;

            float x = (float)outputs[j] * 10.0f;
            float y = (float)outputs[j + 1] * 10.0f;
            float z = (float)(inputs[i] * 10.0f); // Has to be made positive
            braidVectors[i] = new Vector3(x, y, z);

            j += 2;
        }

        return braidVectors;
    }

    public static Vector3[] VectorsToBraidVectors(double[] outputs, int size)
    {
        Vector3[] braidVectors = new Vector3[size];
        for (int i = 0, j = 0; i < size; i++)
        {
            float x = (float)outputs[j] * 10.0f;
            float y = (float)outputs[j + 1] * 10.0f;
            float z = (float)outputs[j + 2] * 10.0f; // Creates interesting braids if z is inputs instead of outputs
            braidVectors[i] = new Vector3(x, y, z);

            j += 3;
        }
        return braidVectors;
    }

    public static double[] Vector3ToDoubleArray(Vector3[] vectors)
    {
        double[] doubleArray = new double[vectors.Length * 3]; 

        for(int i = 0, j = 0; i < vectors.Length; i++)
        {
            doubleArray[j] = vectors[i].x;
            doubleArray[j + 1] = vectors[i].y;
            doubleArray[j + 2] = vectors[i].z;

            j += 3; 
        }

        return doubleArray; 
    }

    public static double[] MergeArraysFromSimpleANN(double[] arr1, double[] arr2)
    {
        int size = arr1.Length + arr2.Length; 
        double[] res = new double[size];

        for (int i = 0, j = 0; i < size; i += 3, j++)
            res[i + 1] = arr1[j];

        for (int i = 0, j = 0; i < size; i += 3, j += 2)
        {
            res[i] = arr2[j];
            res[i + 2] = arr2[j + 1];
        }

        return res; 
    }

    public static double[] MergeArraysFromVectorANN(double[] inputs, double[] deltaValues)
    {
        
        double[] res = new double[deltaValues.Length];
        int i, j; 
        for(i = 0, j = 0; i < inputs.Length; i++, j += 3)
        {

            // TODO: This can be made smarter...
            //double x = (inputs[i] + deltaValues[i] > 1.0) ? inputs[i] + deltaValues[i] : 1.0; 
            double x = 0.0f + deltaValues[j];
            double y = inputs[i] + deltaValues[j + 1];
            double z = 0.0f + deltaValues[j + 2];

            if (x > 1.0) x = 1.0;
            if (x < -1.0) x = 1.0;

            if (y > 1.0) y = 1.0;
            if (y > 1.0) y = -1.0;

            if (z > 1.0) z = 1.0;
            if (z > 1.0) z = -1.0;

            res[j] = x;
            res[j + 1] = y;
            res[j + 2] = z; 
        }

        return res; 
    }

    public static double[] MergeArraysFromCPPNVer2(double[] inputs, double[] deltaValues, double[] matValues, double[] radValues)
    {

        double[] res = new double[inputs.Length];

        for (int i = 0; i < inputs.Length; i += 3)
        {

            // TODO: This can be made smarter...
            //double x = (inputs[i] + deltaValues[i] > 1.0) ? inputs[i] + deltaValues[i] : 1.0; 
            double x = inputs[i] + deltaValues[i];
            double y = inputs[i + 1] + deltaValues[i + 1];
            double z = inputs[i + 2] + deltaValues[i + 2];

            if (x > 1.0) x = 1.0;
            if (x < -1.0) x = 1.0;

            if (y > 1.0) y = 1.0;
            if (y > 1.0) y = -1.0;

            if (z > 1.0) z = 1.0;
            if (z > 1.0) z = -1.0;

            res[i] = x;
            res[i + 1] = y;
            res[i + 2] = z;
        }

        return res;
    }

    public static void NormalizeDeltaValues(double[] deltaArray)
    {

        for(int i = 0; i < deltaArray.Length; i++)
        {
            if (deltaArray[i] <= 0.5)
                deltaArray[i] *= -1; 

            deltaArray[i] /= 10;
        }
    }
}
