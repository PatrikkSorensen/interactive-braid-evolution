using UnityEngine;
using System.Collections;

public class ObjImporter : MonoBehaviour {

    public string objFileName;
    public string objFileName2;

    void Start()
    {
        objFileName = Application.dataPath + "/ObjReader/Sample Files/" + objFileName;
        ObjReader.use.ConvertFile(objFileName, false); 
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Y))
        {
            objFileName = "";
            objFileName = Application.dataPath + "/ObjReader/Sample Files/" + objFileName2;
            ObjReader.use.ConvertFile(objFileName, false);
        }
    }
}
