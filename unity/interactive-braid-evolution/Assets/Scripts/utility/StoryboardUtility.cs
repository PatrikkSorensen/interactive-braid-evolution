using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEditor;
using System.IO;
using DG.Tweening; 

public class StoryboardUtility : MonoBehaviour
{

    private string modelPath;
    private string screenshotPath; 
    private ScreenShotScript ssScript;
    private float rotateTime;
    public Material braidMat;
    public void InitializeStoryboardUtility ()
    {
        ssScript = FindObjectOfType<ScreenShotScript>();
        screenshotPath = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/StoryboardImages/";
        modelPath = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/TempModels/";

        // clean and delete files and folders with temp models
        DirectoryInfo di = new DirectoryInfo(modelPath);

        foreach (FileInfo file in di.GetFiles())
            file.Delete();

        foreach (DirectoryInfo dir in di.GetDirectories())
            dir.Delete(true);

        // clean and delete files and folders with temp images
        di = new DirectoryInfo(screenshotPath);

        foreach (FileInfo file in di.GetFiles())
            file.Delete();

        foreach (DirectoryInfo dir in di.GetDirectories())
            dir.Delete(true);
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
        rotateTime = 1.0f;
        foreach (FileInfo f in storyboardModels)
        {
            // load model 
            GameObject gb = LoadInModel(f.FullName);

            // take screenshot
            gb.transform.position = Camera.main.transform.position + new Vector3(0.0f, 0.0f, 30.0f);
            gb.GetComponent<Renderer>().material = braidMat;
            gb.transform.DORotate(new Vector3(0.0f, 45.0f, 0.0f), rotateTime); 
            yield return new WaitForSeconds(rotateTime); 
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

        FindObjectOfType<XMLFormularWriter>().SaveModels(files.ToArray()); 
        return files; 
    }


}