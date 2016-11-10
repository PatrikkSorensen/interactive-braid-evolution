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

public class Optimizer : MonoBehaviour {

    // Neat parameters
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

    // ANNSetup variables 
    public static ANNSetup ANN_SETUP; 

    void Start ()
    {
        champFileSavePath = Application.dataPath + "/Resources/xml/braid.champ.xml";
        popFileSavePath = Application.dataPath + "/Resources/xml/pop.xml";
    }

    public void InitializeEA()
    {
        
        // set up network structure from dropdown
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = SetupANNStructure();

        // load in XML
        xmlConfig.LoadXml(textAsset.text);

        // set up experiment
        experiment = new BraidExperiment();
        experiment.Initialize("Braid Experiment", xmlConfig.DocumentElement, 0, 0);
        experiment.SetOptimizer(this);

        // set up network variables 
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        if (messenger)
        {
            PopulationSize = XmlUtils.GetValueAsInt(xmlConfig.DocumentElement, "PopulationSize");
            messenger.SetupEvolutionParameters(PopulationSize);
        }

        UnitContainer = GameObject.Find("UnitContainer");
        if (!UnitContainer)
            UnitContainer = new GameObject("UnitContainer");

        // setup the relevant ui
        IECManager.SetUIToEvolvingState();

        if (ANN_SETUP == ANNSetup.VECTOR_BASED)
        {
            Debug.Log("I should create a random list of values");

        }
    }



    public void StartEA()
    {
        Debug.Log("----------------------  SETTING UP EA IN UNITY SCENE ----------------------");
        _ea = experiment.CreateEvolutionAlgorithm(popFileSavePath);
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();

        SetTimeScale();

        Debug.Log("------------------- FINISHED SETTING UP EA -------------------------------");
    }

    void ea_UpdateEvent(object sender, EventArgs e)
    {
        Debug.Log("Generation: " + _ea.CurrentGeneration + ", best fitness: " + _ea.Statistics._maxFitness);
        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;
    }

    void ea_PauseEvent(object sender, EventArgs e)
    {     
        ResetTimeScale();
        SaveXMLFiles(); 
    }

    public void StopEA()
    {
        IECManager.SetUIToExitState(); 
        BraidSelector.SetShouldEvaluate(false);
        BraidSelector.SetReadyForSelection(true); 
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
        controller.CURRENT_GENERATION = (int) _ea.CurrentGeneration; 
        controller.Activate(phenome);
    }

    public void StopEvaluation(IBlackBox box)
    {
        //Debug.Log("Stopping evaluation"); 
        UnitController ct = ControllerMap[box];
        Destroy(ct.gameObject);
    }

    private TextAsset SetupANNStructure()
    {
        TextAsset textAsset;
        ANNSetup setup = UIANNSetupDropdown.GetANNSetup();
        switch (setup)
        {
            case ANNSetup.SIMPLE:
                Debug.Log("Simple setup booted up!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.simple");
                ANN_SETUP = ANNSetup.SIMPLE;
                break;
            case ANNSetup.VECTOR_BASED:
                Debug.Log("Vector Based Setup selected!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.vector");
                ANN_SETUP = ANNSetup.VECTOR_BASED;
                break;
            case ANNSetup.RANDOM_VECTORS:
                Debug.Log("Vector Based Setup selected!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.vector");
                ANN_SETUP = ANNSetup.RANDOM_VECTORS;
                break;
            default:
                Debug.LogError("Something went wrong when getting the network setup");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.random");
                break;
        }

        return textAsset;
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
            Debug.Log("population file saved to disk");
        }

        // Also save the best genome
        using (XmlWriter xw = XmlWriter.Create(champFileSavePath, _xwSettings))
        {
            Debug.Log(_ea.CurrentChampGenome); 
            experiment.SavePopulation(xw, new NeatGenome[] { _ea.CurrentChampGenome });
            Debug.Log("champions file saved to disk");
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
