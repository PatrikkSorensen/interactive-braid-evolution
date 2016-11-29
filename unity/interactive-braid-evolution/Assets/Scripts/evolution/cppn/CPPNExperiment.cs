using System;
using System.Collections.Generic;
using System.Xml;
using SharpNeat.Core;
using SharpNeat.Decoders;
using SharpNeat.Decoders.HyperNeat;
using SharpNeat.DistanceMetrics;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.Genomes.HyperNeat;
using SharpNeat.Genomes.Neat;
using SharpNeat.Network;
using SharpNeat.Phenomes;
using SharpNeat.SpeciationStrategies;
using SharpNeat.Domains;
using SharpNEAT.core;
using UnityEngine;
using SharpNeat.Decoders.Neat;

public class CPPNExperiment : INeatExperiment
{
    NeatEvolutionAlgorithmParameters _eaParams;
    NeatGenomeParameters _neatGenomeParams;
    string _name;
    int _populationSize;
    int _specieCount;
    NetworkActivationScheme _activationScheme;
    string _complexityRegulationStr;
    int? _complexityThreshold;
    string _description;
    Optimizer m_optimizer;
    int _inputCount;
    int _outputCount;

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return _description; }
    }

    public int InputCount
    {
        get { return _inputCount; }
    }

    public int OutputCount
    {
        get { return _outputCount; }
    }

    public int DefaultPopulationSize
    {
        get { return _populationSize; }
    }

    public NeatEvolutionAlgorithmParameters NeatEvolutionAlgorithmParameters
    {
        get { return _eaParams; }
    }

    public NeatGenomeParameters NeatGenomeParameters
    {
        get { return _neatGenomeParams; }
    }

    public void SetOptimizer(Optimizer se)
    {
        this.m_optimizer = se;
    }


    public void Initialize(string name, XmlElement xmlConfig)
    {
        Initialize(name, xmlConfig, 6, 3);
    }

    public void Initialize(string name, XmlElement xmlConfig, int input, int output)
    {
        _name = name;
        _populationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
        _specieCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
        _activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation");
        _complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
        _complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
        _description = XmlUtils.TryGetValueAsString(xmlConfig, "Description");

        _eaParams = new NeatEvolutionAlgorithmParameters();
        _eaParams.SpecieCount = _specieCount;
        _neatGenomeParams = new NeatGenomeParameters(); // Activation functions should be added here? 
        _neatGenomeParams.FeedforwardOnly = _activationScheme.AcyclicNetwork;

        _inputCount = XmlUtils.GetValueAsInt(xmlConfig, "Inputs");
        _outputCount = XmlUtils.GetValueAsInt(xmlConfig, "Outputs");
    }

    public List<NeatGenome> LoadPopulation(XmlReader xr)
    {
        NeatGenomeFactory genomeFactory = (NeatGenomeFactory)CreateGenomeFactory();
        return NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, genomeFactory);
    }

    public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
    {
        NeatGenomeXmlIO.WriteComplete(xw, genomeList, true);
    }

    public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
    {
        return new NeatGenomeDecoder(_activationScheme);
    }

    public IGenomeFactory<NeatGenome> CreateGenomeFactory()
    {
        return new CppnGenomeFactory(_inputCount, _outputCount);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(string fileName)
    {
        List<NeatGenome> genomeList = null;
        IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();
        try
        {
            if (fileName.Contains("/.pop.xml"))
            {
                throw new Exception();
            }
            using (XmlReader xr = XmlReader.Create(fileName))
            {
                genomeList = LoadPopulation(xr);
            }
        }
        catch (Exception e1)
        {
            Utility.Log(fileName + " Error loading genome from file!\nLoading aborted.\n"
                                      + e1.Message + "\nJoe: " + fileName);
            genomeList = genomeFactory.CreateGenomeList(_populationSize, 0);

        }

        return CreateEvolutionAlgorithm(genomeFactory, genomeList);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
    {
        return CreateEvolutionAlgorithm(_populationSize);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
    {
        IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

        List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

        return CreateEvolutionAlgorithm(genomeFactory, genomeList);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
    {
        Debug.Log("Creating CPPN Experiment");
        IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
        ISpeciationStrategy<NeatGenome> speciationStrategy = new KMeansClusteringStrategy<NeatGenome>(distanceMetric);
        IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

        BraidNeatEvolutionAlgorithm<NeatGenome> ea = new BraidNeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

        // Create black box evaluator       
        BraidEvaluator evaluator = new BraidEvaluator(m_optimizer);
        IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = CreateGenomeDecoder();
        IGenomeListEvaluator<NeatGenome> innerEvaluator = new BraidListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, evaluator, m_optimizer);

        ea.Initialize(innerEvaluator, genomeFactory, genomeList);
        return ea;
    }
}