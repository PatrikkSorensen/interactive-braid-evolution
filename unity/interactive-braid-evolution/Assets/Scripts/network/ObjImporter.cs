using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class ObjImporter : MonoBehaviour {

    public bool shouldImport;
    public Transform[] spawnPositions; 

    private string filePathToGeometry;
    private string currFileName; 
    private UIMsgWindow msgWindow;

    void Start()
    {
        // ObjImporter variables
        filePathToGeometry = Application.dataPath + "/Geometrys/";
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

    void Update()
    {
        if (shouldImport) {
            shouldImport = false;
            StartCoroutine(ImportModel(currFileName));
        }
    }

    // Raises flag to start importing model. Assumses folder path and model has been created. 
    public void StartModelImporting(string fileName)
    {
        shouldImport = true;
        currFileName = fileName; 
    }

    IEnumerator ImportModel(string fileName)
    {
        string objFileName = Application.dataPath + "/Geometry/" + fileName;
        GameObject[] models = ObjReader.use.ConvertFile(objFileName, true);

        if (models.Length > 0) {
            // position
            Transform testModel = models[0].transform;
            testModel.position = FindSpawnPosition();

            // rotation
            Vector3 rotationVector = testModel.rotation.eulerAngles;
            rotationVector.x = -90.0f;
            testModel.rotation = Quaternion.Euler(rotationVector);
        } else {
            Debug.LogWarning("No models imported, though a request for it was recieved!");
        }

        yield return new WaitForSeconds(1.0f);
    }

    Vector3 FindSpawnPosition()
    {
        int index = Random.Range(0, spawnPositions.Length);
        Vector3 position = spawnPositions[index].position;
        return position; 
    }
}
