using UnityEngine;
using System.Collections;
using UnityEditor;


public class StoryboardUtility : MonoBehaviour
{

    public static void SaveGenerationData(string[] fileNames)
    {
        Debug.Log("Copying " + fileNames.Length + " models");

        foreach (string s in fileNames)
        {
            Debug.Log(s); 
            if (s != "")
                CopyModel(s);
        }
    }

    public static void CopyModel(string name)
    {
        string sourcePath = Application.dataPath + "/Geometry/Models/" + name + ".obj";
        string destPath = Application.dataPath + "/Geometry/SavedModels/" + name + ".obj"; 

        System.IO.File.Copy(sourcePath, destPath);
        Debug.Log("Copied file from " + sourcePath + " to: " + destPath + " with name: " + name); 
    }

    public static GameObject LoadInModel(string path)
    {
        Debug.Log("Loading in model.."); 
        string objFileName = Application.dataPath + "/Resources/StoryboardImages/Pig_obj.txt";
        GameObject[] curr_model = ObjReader.use.ConvertFile(objFileName, true); // Has to be an array because...? 
        curr_model[0].transform.position = Vector3.zero + new Vector3(-1.0f, 0.0f, 0.0f);
        return curr_model[0]; 
    }
}