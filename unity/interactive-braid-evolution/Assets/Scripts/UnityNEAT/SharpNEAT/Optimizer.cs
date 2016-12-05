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
    //protected BraidExperiment experiment; 
    protected CPPNExperiment experiment; 
    protected static NeatEvolutionAlgorithm<NeatGenome> _ea;
    protected Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();
    public static ANNSetup ANN_SETUP;

    // Evolution parameters
    public static int PopulationSize;
    public static uint Generation;
    protected double Fitness;
    public int Trials;
    public float TrialDuration;
    public float StoppingFitness;

    // Network variables 
    protected ModelMessager messenger; 

    // Utility
    public float evolutionSpeed = 1.0f;
    public bool LoadPopulation = true;
    public GameObject Unit;
    protected GameObject UnitContainer;
    protected string popFileSavePath = null; 
    protected string champFileSavePath = null;


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
        //experiment = new BraidExperiment();
        experiment = new CPPNExperiment();

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
        BraidSimulationManager.populationSize = PopulationSize;
        BraidSimulationManager.evaluationsMade = 0; 

    }



    public void StartEA()
    {
        Debug.Log("----------------------  SETTING UP EA IN UNITY SCENE ----------------------");
        _ea = experiment.CreateEvolutionAlgorithm();
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();

        SetTimeScale();

        Debug.Log("------------------- FINISHED SETTING UP EA -------------------------------");
    }

    protected void ea_UpdateEvent(object sender, EventArgs e)
    {

        Debug.Log("Generation: " + _ea.CurrentGeneration + ", best fitness: " + _ea.Statistics._maxFitness);
        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;
        IECManager.SetGeneration(Generation);
    }

    protected void ea_PauseEvent(object sender, EventArgs e)
    {     
        ResetTimeScale();
        SaveXMLFiles(); 
    }

    public void StopEA()
    {
        Debug.Log("Trying to stop!"); 
        IECManager.SetUIToExitState(); 
        BraidSelector.SetShouldEvaluate(false);
        //BraidSelector.SetReadyToProgressEvolution(false); 

        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            Debug.Log("Stopped!!");
            _ea.Stop();
        } else
        {
            Debug.Log("Couldn't stop...");
        }
    }

    public void Evaluate(IBlackBox phenome)
    {
        // TODO: Setup ids and stuff in the message object
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        BraidController controller = obj.GetComponent<BraidController>();

        /* SPECIFIC TO THE BRAID CONTROLLER EXPERIMENT */ 
        obj.transform.parent = UnitContainer.transform;
        int id = UnitContainer.transform.childCount - 1; 
        obj.name = "unit_" + id;
        controller.BraidId = id;
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

    protected TextAsset SetupANNStructure()
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
            case ANNSetup.CPPN_BASED:
                Debug.Log("CPPN Based Setup selected!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.cppn");
                ANN_SETUP = ANNSetup.CPPN_BASED;
                break;
            case ANNSetup.CPPN_VER2:
                Debug.Log("CPPN Ver2 Setup selected!");
                textAsset = (TextAsset)Resources.Load("experiment.config.braid.cppn.v2");
                ANN_SETUP = ANNSetup.CPPN_VER2;
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

    // Utility functions: 
    protected void SaveXMLFiles()
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
