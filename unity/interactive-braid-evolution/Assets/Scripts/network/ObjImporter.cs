using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class ObjImporter : MonoBehaviour {

    public bool shouldImport;
    public Transform[] spawnPositions; 

    private string filePathToGeometry;
    private string currFileName;
    private int m_num_models; 

    private UIMsgWindow msgWindow;

    void Start()
    {
        // ObjImporter variables
        filePathToGeometry = Application.dataPath + "/Geometry/Models/";
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
            StartCoroutine(ImportModel());
        }
    }

    // Raises flag to start importing model. Assumses folder path and model has been created. 
    public void StartModelImporting(int num_models)
    {
        m_num_models = num_models;
        shouldImport = true; 
    }

    // Assumes file prefix is "braid_" and that models have been exported 
    IEnumerator ImportModel()
    {
        Debug.Log("Starting to importing models..."); 
        shouldImport = false;
        for(int i = 0; i < m_num_models; i++)
        {
            string objFileName = Application.dataPath + "/Geometry/Models/braid_" + i.ToString() + ".obj";
            GameObject[] curr_model = ObjReader.use.ConvertFile(objFileName, true); // Has to be an array because...? 

            if(curr_model != null)
            {
                Debug.Log(objFileName + " was found");

                // position
                Transform testModel = curr_model[0].transform;
                testModel.position = FindSpawnPosition();

                // rotation
                Vector3 rotationVector = testModel.rotation.eulerAngles;
                rotationVector.x = -90.0f;
                testModel.rotation = Quaternion.Euler(rotationVector);

            } else
            {
                Debug.LogError("The model " + objFileName + " could not be found."); 
            }
            yield return new WaitForSeconds(1.0f); 
        }

        /*string objFileName = Application.dataPath + "/Geometry/Models/" + fileName;
        GameObject[] models = ObjReader.use.ConvertFile(objFileName, true);

        if (models != null) {
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

        yield return new WaitForSeconds(1.0f);*/

    }

    Vector3 FindSpawnPosition()
    {
        int index = Random.Range(0, spawnPositions.Length);
        Vector3 position = spawnPositions[index].position;
        return position; 
    }
}
