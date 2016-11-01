using UnityEngine;
using System.Collections;

public class NormalizeHelper : MonoBehaviour {

	private Vector3[] NormalizeInputVectors(Vector3[] vectors)
    {
        float min = -10.0f;
        float max = 10.0f;

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

    /// <summary>
    /// Normalizes values between 0.0 and 40.0f in a range of [-1, 1]
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public static double[] NormalizeInputDoubles(double[] inputs)
    {
        float min = 0.0f;
        float max = 40.0f;

        double[] normalizedArray = new double[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            double x = (inputs[i] - min) / (max - min) * 2 - 1;
            //Debug.Log("input: " + inputs[i] + " became normalized to: " + x); 
            normalizedArray[i] = x;
        }

        return normalizedArray;
    }
}
