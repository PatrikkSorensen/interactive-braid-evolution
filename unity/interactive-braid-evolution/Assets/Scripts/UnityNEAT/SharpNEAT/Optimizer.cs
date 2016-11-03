using UnityEngine;
using SharpNeat.Phenomes;
using SharpNeat.Domains;
using System.Collections.Generic;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using System;
using System.Xml;
using System.IO;

public class Optimizer : MonoBehaviour {

    // Neural Networks
    public int NUM_INPUTS = 3;
    public int NUM_OUTPUTS = 10;

    // Neat parameters
    //SimpleExperiment experiment;
    BraidExperiment experiment; 
    static NeatEvolutionAlgorithm<NeatGenome> _ea;
    Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();

    // Evolution parameters
    private int PopulationSize;
    private uint Generation;
    private double Fitness;
    public int Trials;
    public float TrialDuration;
    public float StoppingFitness;

    // Network variables 
    ModelMessager messenger; 

    // Utility
    public float evolutionSpeed = 1.0f;
    public bool LoadPopulation = true;
    public GameObject Unit;
    private GameObject UnitContainer;
    private DateTime startTime;
    private string popFileSavePath = null; 
    private string champFileSavePath = null;

    public void InitializeEA()
    {
        UIANNSetupDropdown.ANNSetup setup = UIANNSetupDropdown.GetANNSetup();
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset;

        switch (setup)
        {
            case UIANNSetupDropdown.ANNSetup.SIMPLE:
                Debug.Log("Simple setup booted up!");
                textAsset = (TextAsset)Resources.Load("experiment.config");
                break;
            case UIANNSetupDropdown.ANNSetup.VECTOR_BASED:
                Debug.Log("Vector Based Setup selected!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.simple");
                break;
            case UIANNSetupDropdown.ANNSetup.MATERIAL_AND_VECTOR:
                Debug.Log("Material and vector based setup selected");
                textAsset = (TextAsset)Resources.Load("experiment.config");
                break;
            default:
                Debug.LogError("Something went wrong when getting the network setup");
                textAsset = (TextAsset)Resources.Load("experiment.config");
                break;
        }

        // load in XML
        xmlConfig.LoadXml(textAsset.text);

        // set up experiment
        experiment = new BraidExperiment();
        experiment.Initialize("Car Experiment", xmlConfig.DocumentElement, NUM_INPUTS, NUM_OUTPUTS);
        experiment.SetOptimizer(this);

        // set up network variables 
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        if (messenger)
        {
            PopulationSize = XmlUtils.GetValueAsInt(xmlConfig.DocumentElement, "PopulationSize");
            messenger.SetupEvolutionParameters(PopulationSize, UISliderUpdater.GetValue("Height"));
        }
        else
        {
            Debug.LogError("No network messenger found in scene!");
        }

        // set up utility variables
        champFileSavePath = Application.persistentDataPath + string.Format("/{0}.champ.xml", "car");
        if (LoadPopulation)
            popFileSavePath = Application.persistentDataPath + string.Format("/{0}.pop.xml", "car");

        startTime = DateTime.Now;

        UnitContainer = GameObject.Find("UnitContainer");
        if (!UnitContainer)
            UnitContainer = new GameObject("UnitContainer");

        // setup the relevant ui
        IECManager.SetUIToEvolvingState(); 
    }


    public void StartEA()
    {
        Debug.Log("----------------------  SETTING UP EA IN UNITY SCENE ----------------------");
        // ea and neat 
        _ea = experiment.CreateEvolutionAlgorithm(popFileSavePath);
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();

        // evaluation and braid controller related 
        SetTimeScale();

        Debug.Log("------------------- FINISHED SETTING UP EA -------------------------------");
    }

    void ea_UpdateEvent(object sender, EventArgs e)
    {
        //Debug.Log("Generation: " + _ea.CurrentGeneration + ", best fitness: " + _ea.Statistics._maxFitness);

        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;
        
    }

    void ea_PauseEvent(object sender, EventArgs e)
    {
        Debug.Log("EA paused!"); 
        ResetTimeScale();
        SaveXMLFiles(); 
    }

    public void StopEA()
    {
        Debug.Log("EA stopped!");
        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            _ea.Stop();
        }
    }

    public void Evaluate(IBlackBox phenome)
    {
        // TODO: Setup ids and stuff in the message object
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        BraidController controller = obj.GetComponent<BraidController>();

        /* SPECIFIC TO THE BRAID CONTROLLER EXPERIMENT */ 
        obj.transform.parent = UnitContainer.transform;
        obj.name = "unit_" + UnitContainer.transform.childCount;
        controller.BraidId = UnitContainer.transform.childCount;
        /* END OF SPECIFIC OPERATIONS TO THE BRAID EXPERIMENT */

        ControllerMap.Add(phenome, controller);
        controller.Activate(phenome);
    }

    public void StopEvaluation(IBlackBox box)
    {
        Debug.Log("Stopping evaluation"); 
        UnitController ct = ControllerMap[box];
        Destroy(ct.gameObject);
    }

    public void RunBest()
    {
        ResetTimeScale();

        NeatGenome genome = LoadGenome();

        // Get a genome decoder that can convert genomes to phenomes.
        var genomeDecoder = experiment.CreateGenomeDecoder();

        // Decode the genome into a phenome (neural network).
        var phenome = genomeDecoder.Decode(genome);

        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        ControllerMap.Add(phenome, controller);
        controller.Activate(phenome);
    }

    public NeatGenome LoadGenome ()
    {
        NeatGenome genome = null;

        using (XmlReader xr = XmlReader.Create(champFileSavePath))
            genome = NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, (NeatGenomeFactory)experiment.CreateGenomeFactory())[0];

        return genome; 
    }

    public float GetFitness(IBlackBox box)
    {
        if (ControllerMap.ContainsKey(box))
        {
            return ControllerMap[box].GetFitness();
        }
        return 0;
    }

    public void SetEAProgressFlag(bool flag)
    {
        Debug.Log("Input recieved..."); 
        
        _ea.SetProgressFlag(flag); 
    }

    // Utility functions: 
    void SaveXMLFiles()
    {
        XmlWriterSettings _xwSettings = new XmlWriterSettings();
        _xwSettings.Indent = true;

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath);
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        using (XmlWriter xw = XmlWriter.Create(popFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, _ea.GenomeList);
        }

        // Also save the best genome
        using (XmlWriter xw = XmlWriter.Create(champFileSavePath, _xwSettings))
        {
            experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
        }
        DateTime endTime = DateTime.Now;
        Utility.Log("Total time elapsed: " + (endTime - startTime));
    }

    // time functions
    void SetTimeScale()
    {
        Time.timeScale = evolutionSpeed;
    }

    void ResetTimeScale()
    {
        Time.timeScale = 1; 
    }
}
