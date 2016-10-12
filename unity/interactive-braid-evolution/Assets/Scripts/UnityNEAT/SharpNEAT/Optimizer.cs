using UnityEngine;
using System.Collections;
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
    public int NUM_OUTPUTS = 3;

    // Neat parameters
    SimpleExperiment experiment;
    static NeatEvolutionAlgorithm<NeatGenome> _ea;
    Dictionary<IBlackBox, UnitController> ControllerMap = new Dictionary<IBlackBox, UnitController>();

    // Evolution parameters
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
    //private float timeLeft;
    //private float accum;
    //private int frames;
    //private float updateInterval = 12;
    private string popFileSavePath = null; 
    private string champFileSavePath = null;



    void Start () {
        // load in XML
        XmlDocument xmlConfig = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("experiment.config");
        xmlConfig.LoadXml(textAsset.text);

        // set up experiment
        experiment = new SimpleExperiment();
        experiment.Initialize("Car Experiment", xmlConfig.DocumentElement, NUM_INPUTS, NUM_OUTPUTS);
        experiment.SetOptimizer(this);

        // set up network variables 
        messenger = GameObject.FindObjectOfType<ModelMessager>();
        if (messenger)
        {
            int populationSize = XmlUtils.GetValueAsInt(xmlConfig.DocumentElement, "PopulationSize");
            messenger.SetupEvolutionParameters(populationSize, 5);
        } else
        {
            Debug.LogError("No network messenge found in scene!");
        }

        // set up utility variables
        champFileSavePath = Application.persistentDataPath + string.Format("/{0}.champ.xml", "car");
        if(LoadPopulation)
            popFileSavePath = Application.persistentDataPath + string.Format("/{0}.pop.xml", "car");
        print(champFileSavePath);

        startTime = DateTime.Now;

        UnitContainer = GameObject.Find("UnitContainer");
        if (!UnitContainer)
            UnitContainer = new GameObject("UnitContainer");


    }

    public void StartEA()
    {
        SetTimeScale();
       //essenger.SetupEvolutionParameters(_)

        _ea = experiment.CreateEvolutionAlgorithm(popFileSavePath);
        _ea.UpdateEvent += new EventHandler(ea_UpdateEvent);
        _ea.PausedEvent += new EventHandler(ea_PauseEvent);
        _ea.StartContinue();
    }

    void ea_UpdateEvent(object sender, EventArgs e)
    {
        Debug.Log("Generation: " + _ea.CurrentGeneration + ", best fitness: " + _ea.Statistics._maxFitness);

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
        GameObject obj = Instantiate(Unit, Unit.transform.position, Unit.transform.rotation) as GameObject;
        UnitController controller = obj.GetComponent<UnitController>();

        obj.transform.parent = UnitContainer.transform;
        obj.name = "unit_" + Generation.ToString() + "_" + UnitContainer.transform.childCount; 

        ControllerMap.Add(phenome, controller);
        controller.Activate(phenome);
    }

    public void StopEvaluation(IBlackBox box)
    {
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

        GUI.Button(new Rect(10, Screen.height - 70, 100, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", Generation, Fitness));
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

        System.IO.StreamReader stream = new System.IO.StreamReader(popFileSavePath);
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
