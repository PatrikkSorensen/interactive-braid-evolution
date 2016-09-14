using UnityEngine;
using System.Collections;

public class ObjImporter : MonoBehaviour {

    public string objFileName;
    public string objFileName2;

    private UIMsgWindow msgWindow; 

    void Start()
    {
        msgWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgWindow>();

        objFileName = Application.dataPath + "/Geometrys/" + objFileName;
        msgWindow.AddMessage("Filepath: " + Application.dataPath + "/Geometrys/");

        if (ObjReader.use.ConvertFile(objFileName, false) == null)
            msgWindow.AddMessage("No models loaded"); 
        else
            Debug.Log("models was loaded");  
    }

    public void LoadModel(string fileName)
    {
        objFileName = "";
        objFileName = Application.dataPath + "/Geometry/" + objFileName2;
        ObjReader.use.ConvertFile(objFileName, false);
    }
}
