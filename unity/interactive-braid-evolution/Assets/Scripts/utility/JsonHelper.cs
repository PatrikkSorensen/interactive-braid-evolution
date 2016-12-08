using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

public class JsonHelper : MonoBehaviour {

    /// <summary>
    ///  Creates JSON object that contains list of braids. 
    /// </summary>
    /// <param name="height"></param>
    /// <param name="populationSize"></param>
    /// <param name="braids"></param>
    /// <returns></returns>
    public static string CreateJSONFromBraids(int populationSize, Braid[] braids = null, int height = 10)
    {      
        // Add it all to the json object
        var jo = new JObject();
        jo.Add("population_size", populationSize);
        jo.Add("height", height);
        jo.Add("braids", JToken.FromObject(braids)); 
        var json = jo.ToString();

        return json; 
    }

    public static string CreateJSONFromDataTree(BraidNode root)
    {
        Debug.Log("Trying to create JSON from data tree");

        var jo = new JObject();
        List<Vector3> vectors = new List<Vector3>(); 
        List<BraidNode> firstStack = new List<BraidNode>();

        firstStack.Add(root);

        List<List<BraidNode>> childListStack = new List<List<BraidNode>>();
        childListStack.Add(firstStack);

        while (childListStack.Count > 0)
        {
            List<BraidNode> childStack = childListStack[childListStack.Count - 1];

            if (childStack.Count == 0)
            {
                childListStack.RemoveAt(childListStack.Count - 1);
            }
            else
            {
                root = childStack[0];
                childStack.RemoveAt(0);

                vectors.Add(root.data.vector); 

                if (root.children.Count > 0)
                {
                    childListStack.Add(new List<BraidNode>(root.children));
                }
            }
        }

        Braid[] braids = new Braid[9];
        double[] radiusArray = new double[9];

        for (int i = 0; i < 9; i++)
            radiusArray[i] = 1.0; 

        for (int i = 0; i < 9; i++)
            braids[i] = new Braid("braid_" + i.ToString(), vectors.ToArray(), null, radiusArray); 

        return CreateJSONFromBraids(9, braids, 10); 
    }

    public static string CreateJSONFromVectors(List<Vector3[]> braidVectors)
    {
        Braid[] braids = new Braid[9];
        double[] radiusArray = new double[9];

        for (int i = 0; i < 9; i++)
            radiusArray[i] = 1.0;

        

        for (int i = 0; i < 9; i++)
            braids[i] = new Braid("braid_" + i.ToString(), braidVectors, null, radiusArray);


        return CreateJSONFromBraids(9, braids);
    }

}
