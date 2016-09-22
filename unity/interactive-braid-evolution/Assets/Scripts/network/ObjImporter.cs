using UnityEngine;
using System.Collections;

public class ObjImporter : MonoBehaviour {

    public string objFileName;
    public string objFileName2;
    public bool shouldImport;

    private string filePathToGeometry;
    private string currFileName; 
    private UIMsgWindow msgWindow; 

    void Start()
    {
        // ObjImporter variables
        filePathToGeometry = Application.dataPath + "/Geometrys/";
        objFileName = filePathToGeometry + objFileName;
        shouldImport = false;
        currFileName = null; 

        // UI Message window
        if (GameObject.FindGameObjectWithTag("UIManager")) {
            msgWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgWindow>();
            msgWindow.AddMessage("Filepath: " + filePathToGeometry);
        } else {
            Debug.LogWarning("No UI Messenger found");
        }
    }

    IEnumerator LoadModel(string fileName)
    {
        Debug.Log("Trying to load model: " + fileName);
        objFileName = filePathToGeometry + fileName;
        ObjReader.use.ConvertFile(objFileName, false);
        yield return null; 
    }

    void Update()
    {
        if (shouldImport) {
            shouldImport = false; 
            StartCoroutine(ImportModel(currFileName));
        }
    }

    public void StartModelImporting(string fileName)
    {
        shouldImport = true;
        currFileName = fileName; 
    }

    IEnumerator ImportModel(string fileName)
    {
        objFileName = Application.dataPath + "/Geometry/" + fileName;
        ObjReader.use.ConvertFile(objFileName, true);
        yield return null;
    }
}
