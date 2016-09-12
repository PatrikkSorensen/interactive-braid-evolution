using UnityEngine;
using System.Collections;

public class ObjImporter : MonoBehaviour {

    public string objFileName;
    public string objFileName2;

    void Start()
    {
        objFileName = Application.dataPath + "/Geometrys/" + objFileName;

        if (ObjReader.use.ConvertFile(objFileName, false) == null)
            Debug.Log("No models loaded");
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
