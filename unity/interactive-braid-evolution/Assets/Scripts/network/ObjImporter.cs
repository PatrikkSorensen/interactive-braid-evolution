using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class ObjImporter : MonoBehaviour {

    public bool shouldImportAll;
    public bool shouldImportSingle; 
    public Transform[] spawnPositions;
    public float tweenDuration = 2.0f;
    public float offsetY = -10.0f;
    public Material braidMaterial; 

    private string filePathToGeometry;
    private int m_num_models;
    private int m_curr_index;
    private string m_file_name;
    private int spawn_id; 
    void Start()
    {
        spawn_id = 0; 
        filePathToGeometry = Application.dataPath + "/Geometry/Models/";
        shouldImportAll = false;
        shouldImportSingle = false; 
    }

    void Update()
    {
        if (shouldImportSingle && m_file_name != "")
        {
            StartCoroutine(ImportModel(m_file_name)); 
        } else if(shouldImportSingle)
        {
            StartCoroutine(ImportModel(m_curr_index));
        }
    }

    // Raises flag to start importing model. Assumses folder path and model has been created. 
    public void StartImportingAllModels(int num_models)
    {
        m_num_models = num_models;
        shouldImportAll = true;
    }

    public void StartImportSingleModel(int i)
    {
        UIStatusWindow.modelsImported++;
        shouldImportSingle = true; 
        m_curr_index = i; 
    }

    public void StartImportSingleModel(string file)
    {
        shouldImportSingle = true;
        m_file_name = file; 
    }


    IEnumerator ImportModel(string file)
    {
        m_file_name = ""; 
        shouldImportSingle = false;
        string objFileName = Application.dataPath + "/Geometry/Models/" + file + ".obj";
        GameObject[] models = ObjReader.use.ConvertFile(objFileName, true);
        GameObject curr_model = models[0];

        if (models == null)
            Debug.LogWarning("No model imported from obj importer...");
        else
        {
            // names and id
            curr_model.name = file;
            curr_model.tag = "Braid";
            curr_model.GetComponent<MeshRenderer>().material = braidMaterial;

            // position
            Transform testModel = curr_model.transform;
            Vector3 v = FindSpawnPosition();

            // tweening
            testModel.position = v + Vector3.up * offsetY;
            testModel.DOMove(v, tweenDuration);

            // collision box for selection
            curr_model.AddComponent<BoxCollider>();

            // components and other scripts
            curr_model.AddComponent<Rotate>();
            curr_model.AddComponent<MaterialScript>();

            // ui 
            Debug.Log("imported..."); 
            UIStatusWindow.modelsImported++; 
        }
        
        yield return new WaitForSeconds(0.05f);
        FindObjectOfType<ModelMessenger>().modelling = false;
    }

    IEnumerator ImportModel(int index)
    {
        //Debug.Log("Starting to import a single model with index: " + index);
        shouldImportSingle = false;
        string objFileName = Application.dataPath + "/Geometry/Models/braid_" + index.ToString() + ".obj";
        GameObject[] models = ObjReader.use.ConvertFile(objFileName, true);
        GameObject curr_model;

        if (models == null)
            Debug.LogWarning("No model imported from obj importer...");
        else
        {
            curr_model = models[0];
            // names and id
            curr_model.name = "braid_" + index.ToString();
            curr_model.tag = "Braid";

            // position
            Transform testModel = curr_model.transform;
            Vector3 v = FindSpawnPosition(index);

            // tweening
            testModel.position = v + Vector3.up * offsetY;
            testModel.DOMove(v, tweenDuration);

            // rotation
            Vector3 rotationVector = testModel.rotation.eulerAngles;
            //rotationVector.x = -90.0f;
            testModel.rotation = Quaternion.Euler(rotationVector);

            // collision box for selection
            curr_model.AddComponent<BoxCollider>();

            // components and other scripts
            curr_model.AddComponent<Rotate>();
            curr_model.AddComponent<MaterialScript>(); 
        }

        yield return new WaitForSeconds(0.02f);
    }

    Vector3 FindRandomSpawnPosition()
    {
        int index = Random.Range(0, spawnPositions.Length);
        Vector3 position = spawnPositions[index].position;
        return position; 
    }

    Vector3 FindSpawnPosition()
    {
        if (spawn_id >= spawnPositions.Length)
            spawn_id = 0; 

        return spawnPositions[spawn_id++].position; 
    }

    Vector3 FindSpawnPosition(int index)
    {
        return spawnPositions[index].position; 
    }

    // Assumes file prefix is "braid_" and that models have been exported 
    //IEnumerator ImportAllModels()
    //{
    //    Debug.Log("Starting to import models...");
    //    shouldImportAll = false;
    //    for(int i = 0; i < m_num_models; i++)
    //    {
    //        string objFileName = filePathToGeometry + "/braid_" + i.ToString() + ".obj";
    //        GameObject[] curr_model = ObjReader.use.ConvertFile(objFileName, true); // Has to be an array because...? 

    //        if(curr_model != null)
    //        {
    //            Debug.Log(objFileName + " was found");

    //            // position
    //            Transform testModel = curr_model[0].transform;
    //            testModel.position = FindSpawnPosition();

    //            // rotation
    //            //Vector3 rotationVector = testModel.rotation.eulerAngles;
    //            //rotationVector.x = -90.0f;
    //            //testModel.rotation = Quaternion.Euler(rotationVector);

    //        } else
    //        {
    //            Debug.LogError("The model " + objFileName + " could not be found."); 
    //        }
    //        yield return new WaitForSeconds(1.0f); 
    //    }
    //}

}
