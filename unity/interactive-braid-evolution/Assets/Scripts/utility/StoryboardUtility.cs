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
    public static int m_generation; 

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

            m_generation = generation; 
        }
    }

    public static void CopyModel(string name, int folder)
    {

        string sourcePath = Application.dataPath + "/Geometry/Models/" + name + ".obj";
        string destPath = Application.dataPath + "/Geometry/TempModels/" + folder.ToString(); 

        if(File.Exists(destPath))
            Debug.Log("Folder already exists!"); 
        else
            Directory.CreateDirectory(destPath); 

        File.Copy(sourcePath, destPath + "/" + name + ".obj");
    }

    public static GameObject LoadInModel(string path)
    {
        GameObject[] curr_model = ObjReader.use.ConvertFile(path, true); 
        return curr_model[0]; 
    }

    public void StartStoryBoardStep()
    {
        StartCoroutine(CreateScreenShots());
    }

    public IEnumerator CreateScreenShots()
    {
        Dictionary<int, FileInfo[]> storyboardModels = LoadModels(modelPath);
        ScreenShotScript ssScript = FindObjectOfType<ScreenShotScript>();
        rotateTime = 0.35f;

        Vector3 offset;

        if (GameObject.Find("LeapEventSystem"))
            offset = new Vector3(0.0f, 0.0f, 20.0f); 
        else
            offset = new Vector3(0.0f, 0.0f, 100.0f);

        foreach (int key in storyboardModels.Keys)
        {
            foreach(FileInfo f in storyboardModels[key])
            {
                GameObject gb = LoadInModel(f.FullName);


                gb.transform.position = Camera.main.transform.position + offset;
                gb.GetComponent<Renderer>().material = braidMat;
                gb.transform.DORotate(new Vector3(0.0f, 45.0f, 0.0f), rotateTime);
                yield return new WaitForSeconds(rotateTime);

                // take screenshot
                StartCoroutine(ssScript.CreateRenderTexture(gb, key));
                yield return new WaitForSeconds(0.2f);
            }
        }

        ssScript.CreateStoryboardUI(); 

    }

    public Dictionary<int, FileInfo[]> LoadModels(string filePath)
    {
        Dictionary<int, FileInfo[]> models = new Dictionary<int, FileInfo[]>(); 
        List<FileInfo> files = new List<FileInfo>(); 
        DirectoryInfo di = new DirectoryInfo(filePath);

        int i = 0; 
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            foreach(FileInfo file in dir.GetFiles())
            {
                if (!file.Name.Contains(".obj.meta"))
                    files.Add(file); 
            }
            models.Add(i, files.ToArray());
            files.Clear(); 
            i++;
        }

        return models; 
    }


}