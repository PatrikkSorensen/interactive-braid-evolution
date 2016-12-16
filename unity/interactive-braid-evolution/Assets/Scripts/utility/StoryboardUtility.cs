using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class StoryboardUtility : MonoBehaviour
{

    private string filePath;

    private void Start()
    {
        filePath = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/SavedModels"; 
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
            StartStoryBoardStep(); 
    }

    public void CleanUpFolders ()
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(filePath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    public static void SaveGenerationData(string[] fileNames, int generation)
    {
        Debug.Log("Copying " + fileNames.Length + " models. Generation: " + generation);

        foreach (string s in fileNames)
        {
            //Debug.Log(s); 
            if (s != "")
                CopyModel(s, generation);
        }
    }

    public static void CopyModel(string name, int folder)
    {

        string sourcePath = Application.dataPath + "/Geometry/Models/" + name + ".obj";
        string destPath = Application.dataPath + "/Geometry/SavedModels/" + folder.ToString(); 

        if(File.Exists(destPath))
        {
            Debug.Log("Folder already exists!"); 
        } else
        {
            Directory.CreateDirectory(destPath); 
        }

        File.Copy(sourcePath, destPath + "/" + name + ".obj");
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

    public void StartStoryBoardStep()
    {
        Debug.Log("Creating storyboard"); 
        // create storyboard

        // trigger anim 

        // save xml 

        // return to splash 
    }


}