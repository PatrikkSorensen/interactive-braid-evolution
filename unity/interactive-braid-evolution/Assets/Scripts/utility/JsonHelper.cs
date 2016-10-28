using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class JsonHelper : MonoBehaviour {

    /// <summary>
    ///  Creates JSON object that contains list of braids. 
    /// </summary>
    /// <param name="height"></param>
    /// <param name="populationSize"></param>
    /// <param name="braids"></param>
    /// <returns></returns>
    public static string CreateJSONFromBraids(int height, int populationSize, Braid[] braids = null)
    {      
        // Add it all to the json object
        var jo = new JObject();
        jo.Add("population_size", populationSize);
        jo.Add("height", height);
        jo.Add("braids", JToken.FromObject(braids)); 
        var json = jo.ToString();

        return json; 
    }
}
