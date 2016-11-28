using UnityEngine;
using System.Collections;
using UnityEditor;


public class ModelCopyScript : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            ModelCopyScript.CopyModels("copied_file"); 
        }
    }

    static void CopyModels(string name)
    {
        string sourcePath = Application.dataPath + "/Testing/screenshot_0.png";
        string destPath = Application.dataPath + "/Geometry/" + name; 

        System.IO.File.Copy(sourcePath, destPath); 
    }
}