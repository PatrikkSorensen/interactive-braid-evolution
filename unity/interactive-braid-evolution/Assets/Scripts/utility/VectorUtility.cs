using UnityEngine;
using System.Collections;

public class VectorUtility : MonoBehaviour {


    public static Vector3[] CreateRandomVectors(int count)
    {
        Vector3[] vectors = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);
            float z = Random.Range(-1.0f, 1.0f);

            Vector3 v = new Vector3(x, y, z);
            vectors[i] = v; 
        }

        foreach (Vector3 v in vectors)
            Debug.Log("Vector: " + v); 

        return vectors;
    }
}
