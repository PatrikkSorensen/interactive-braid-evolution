using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEditor;
using System.IO;

public class StoryboardUtility : MonoBehaviour
{

    private string modelPath;
    private string screenshotPath; 
    private ScreenShotScript ssScript; 

    private void Start()
    {

        screenshotPath = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/StoryboardImages/";
        ssScript = FindObjectOfType<ScreenShotScript>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
            StartStoryBoardStep(); 
    }

    public void CleanUpFolders ()
    {
        modelPath = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/TempModels/";
        DirectoryInfo di = new DirectoryInfo(modelPath);

        foreach (FileInfo file in di.GetFiles())
        {
            Debug.Log("Some file here? " + file.FullName);
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            Debug.Log("Some dir here? " + dir.FullName); 
            dir.Delete(true);
        }

        Debug.Log("Done deleting.."); 
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
        string destPath = Application.dataPath + "/Geometry/TempModels/" + folder.ToString(); 

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
        GameObject[] curr_model = ObjReader.use.ConvertFile(path, true); // Has to be an array because...? 
        return curr_model[0]; 
    }

    public void StartStoryBoardStep()
    {
        Debug.Log("Creating storyboard");

        StartCoroutine(CreateScreenShots());
        // create storyboard

        // trigger anim 

        // save xml 

        // return to splash 
    }

    public IEnumerator CreateScreenShots()
    {
        List<FileInfo> storyboardModels = LoadModels(modelPath);
        ScreenShotScript ssScript = FindObjectOfType<ScreenShotScript>();

        foreach (FileInfo f in storyboardModels)
        {
            // load model 
            GameObject gb = LoadInModel(f.FullName);

            // take screenshot
            StartCoroutine(ssScript.CreateRenderTexture(gb));
            yield return new WaitForSeconds(0.2f);

            Debug.Log("Creating new screenshot!"); 
        }



        // load in screenshots 
        ssScript.CreateStoryboardUI(); 

    }

    public List<FileInfo> LoadModels(string filePath)
    {
        List<FileInfo> files = new List<FileInfo>(); 
        DirectoryInfo di = new DirectoryInfo(filePath);

        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            foreach(FileInfo file in dir.GetFiles())
            {
                if (!file.Name.Contains(".obj.meta"))
                    files.Add(file); 
            }
        }

        return files; 
    }


}