using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class JsonHelper : MonoBehaviour {
    public static string CreateJSONFromBraids(int height, int populationSize, Braid[] braids = null)
    {
        if (braids == null)
            return " "; 


      
        // Add it all to the json object
        var jo = new JObject();
        jo.Add("population_size", populationSize);
        jo.Add("height", height);
        jo.Add("braids", JToken.FromObject(braids)); 
        var json = jo.ToString();

        return json; 
    }
}
