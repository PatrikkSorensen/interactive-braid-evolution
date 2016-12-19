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

    // Neat parameters
    //protected BraidExperiment experiment; 
    protected CPPNExperiment experiment; 
    protected static NeatEvolutionAlgorithm<NeatGenome> _ea;
    protected Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();

    // Evolution parameters
    public static int PopulationSize;
    public static uint Generation;
    protected double Fitness;
    public int Trials;
    public float TrialDuration;
    public float StoppingFitness;

    // Network variables 
    protected ModelMessenger messenger; 

    // Utility
    public float evolutionSpeed = 1.0f;
    public bool LoadPopulation = true;
    public GameObject Unit;
    protected GameObject UnitContainer;
    protected string popFileSavePath = null;
    protected string popLoadSavePath = null;
    protected string champFileSavePath = null;


    void Start ()
    {

        champFileSavePath = Application.dataPath + "/Resources/xml/braid.champ.xml";
        popFileSavePath = Application.dataPath + "/Resources/xml/pop.xml";
        popLoadSavePath = Application.dataPath + "/Resources/xml/startup_populations/pop.cppn.split.xml";
    }

    public void InitializeEA()
    {

        // set up network structure from dropdown
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("ExperimentSetups/experiment.config.braid.cppn.split");

        // load in XML
        xmlConfig.LoadXml(textAsset.text);

        // set up experiment
        experiment = new CPPNExperiment();

        experiment.Initialize("Braid Experiment", xmlConfig.DocumentElement, 0, 0);
        experiment.SetOptimizer(this);

        // set up network variables 
        messenger = GameObject.FindObjectOfType<ModelMessenger>();
        PopulationSize = XmlUtils.GetValueAsInt(xmlConfig.DocumentElement, "PopulationSize");
        messenger.SetupEvolutionParameters(PopulationSize);

        UnitContainer = GameObject.Find("UnitContainer");
        if (!UnitContainer)
            UnitContainer = new GameObject("UnitContainer");

        // setup the relevant ui
        IECManager.SetUIToEvolvingState();
        BraidSimulationManager.populationSize = PopulationSize;
        BraidSimulationManager.evaluationsMade = 0;

        // clean up folders
        FindObjectOfType<StoryboardUtility>().InitializeStoryboardUtility();

        // start the EA 
        Generation = 0;
        StartEA(); 
    }

    public void Evaluate(IBlackBox phenome)
    {
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        BraidController controller = obj.GetComponent<BraidController>();

        obj.transform.parent = UnitContainer.transform;
        int id = UnitContainer.transform.childCount - 1; 
        obj.name = "unit_" + id;
        controller.BraidId = id;

        ControllerMap.Add(phenome, controller);
        controller.CURRENT_GENERATION = (int) _ea.CurrentGeneration; 
        controller.Activate(phenome);
    }

    public void StopEvaluation(IBlackBox box)
    {
        UnitController ct = ControllerMap[box];
        Destroy(ct.gameObject);
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

    public void StartEA()
    {
        _ea = experiment.CreateEvolutionAlgorithm(popLoadSavePath);
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();
    }

    protected void ea_UpdateEvent(object sender, EventArgs e)
    {
        Fitness = _ea.Statistics._maxFitness;
        Generation = _ea.CurrentGeneration;
        IECManager.SetGeneration(Generation);
    }

    protected void ea_PauseEvent(object sender, EventArgs e)
    {
        SaveXMLFiles();
    }

    public void StopEA()
    {


        if (_ea != null && _ea.RunState == SharpNeat.Core.RunState.Running)
        {
            _ea.Stop();
            Debug.Log("Stoping EA");
            IECManager.SetUIToExitState();
            BraidSimulationManager.SetShouldBraidsEvaluate(false);
        }
    }


    // Utility functions: 
    protected void SaveXMLFiles()
    {
        XmlWriterSettings _xwSettings = new XmlWriterSettings();
        _xwSettings.Indent = true;
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath);
        if (!dirInf.Exists)
        {
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
            Debug.Log("champions file saved to disk");
        }
    }
}
