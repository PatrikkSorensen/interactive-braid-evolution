using UnityEngine;
using SharpNeat.Phenomes;
using SharpNeat.Domains;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;
using ExperimentTypes;

public class CPPNOptimizer : Optimizer {

    protected new CPPNExperiment experiment;

    public new void InitializeEA()
    {
        Debug.Log("Cppn optimizer initialized"); 

        // set up network structure from dropdown
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = textAsset = (TextAsset)Resources.Load("experiment.config.braid.cppn");

        // load in XML
        xmlConfig.LoadXml(textAsset.text);

        // set up experiment
        experiment = new CPPNExperiment();
        experiment.Initialize("Braid Experiment", xmlConfig.DocumentElement, 0, 0);
        experiment.SetOptimizer(this);
    }

    public new void Evaluate(IBlackBox phenome)
    {
        // TODO: Setup ids and stuff in the message object
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        CPPNController controller = obj.GetComponent<CPPNController>();

        /* SPECIFIC TO THE BRAID CONTROLLER EXPERIMENT */
        UnitContainer = GameObject.Find("UnitContainer"); 
        obj.transform.parent = UnitContainer.transform;
        int id = UnitContainer.transform.childCount - 1;
        obj.name = "unit_" + id;
        /* END OF SPECIFIC OPERATIONS TO THE BRAID EXPERIMENT */

        ControllerMap.Add(phenome, controller);
        //controller.CURRENT_GENERATION = (int)_ea.CurrentGeneration;
        controller.Activate(phenome);
    }


    public new void StartEA()
    {
        InitializeEA(); 
        Debug.Log("----------------------  SETTING UP EA IN UNITY SCENE ----------------------");
        _ea = experiment.CreateEvolutionAlgorithm();
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();

        Debug.Log("------------------- FINISHED SETTING UP EA -------------------------------");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 40), "Start EA"))
        {
            StartEA();
        }
        if (GUI.Button(new Rect(10, 60, 100, 40), "Stop EA"))
        {
           StopEA();
        }
        if (GUI.Button(new Rect(10, 110, 100, 40), "Run best"))
        {
            RunBest();
        }

        GUI.Button(new Rect(10, Screen.height - 70, 100, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", Generation, 0.0f));
    }

    new void StopEA()
    {

        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            Debug.Log("Stopped!!");
            SaveXMLFiles(); 
            _ea.Stop();
        }
        else
        {
            Debug.Log("Couldn't stop...");
        }
    }

    new void SaveXMLFiles()
    {
        XmlWriterSettings _xwSettings = new XmlWriterSettings();
        _xwSettings.Indent = true;
        Debug.Log("Save xml called");
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath);
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        using (XmlWriter xw = XmlWriter.Create(popFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, _ea.GenomeList);
            Debug.Log("population file saved to disk");
        }

        // Also save the best genome
        using (XmlWriter xw = XmlWriter.Create(champFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
            Debug.Log("champions file saved to disk");
        }
    }


}
