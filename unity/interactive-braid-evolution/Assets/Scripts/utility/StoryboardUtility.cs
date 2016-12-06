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
}