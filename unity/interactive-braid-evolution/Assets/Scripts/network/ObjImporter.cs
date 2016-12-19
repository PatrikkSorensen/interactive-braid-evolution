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
            Transform t = FindSpawnPosition();
            curr_model.transform.SetParent(t);

            // tweening
            testModel.position = t.position + Vector3.up * offsetY;
            testModel.DOMove(t.position, tweenDuration);

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
        Debug.Log("Starting to import a single model with index: " + index);
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
            Transform t = FindSpawnPosition(index);
            curr_model.transform.SetParent(t); 

            // tweening
            testModel.position = t.position + Vector3.up * offsetY;
            testModel.DOMove(t.position, tweenDuration);

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

    Transform FindSpawnPosition()
    {
        if (spawn_id >= spawnPositions.Length)
            spawn_id = 0; 

        return spawnPositions[spawn_id++]; 
    }

    Transform FindSpawnPosition(int index)
    {
        return spawnPositions[index]; 
    }
}
