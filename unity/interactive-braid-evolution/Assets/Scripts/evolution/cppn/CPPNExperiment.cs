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

public class CPPNExperiment : INeatExperiment
{
    NeatEvolutionAlgorithmParameters _eaParams;
    NeatGenomeParameters _neatGenomeParams;
    string _name;
    int _populationSize;
    int _specieCount;
    NetworkActivationScheme _activationSchemeCppn;
    NetworkActivationScheme _activationScheme;
    string _complexityRegulationStr;
    int? _complexityThreshold;
    string _description;
    bool _lengthCppnInput;
    Optimizer m_optimizer;

    public string Name
    {
        get { return _name; }
    }

    public string Description
    {
        get { return _description; }
    }

    // TODO: Needs to change accordingly to the domain 
    public int InputCount
    {
        get { return _lengthCppnInput ? 7 : 6; }
    }

    // TODO: Needs to change accordingly to the domain 
    public int OutputCount
    {
        get { return 2; }
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

    public void Initialize(string name, XmlElement xmlConfig)
    {
        _name = name;
        _populationSize = XmlUtils.GetValueAsInt(xmlConfig, "PopulationSize");
        _specieCount = XmlUtils.GetValueAsInt(xmlConfig, "SpecieCount");
        _activationSchemeCppn = ExperimentUtils.CreateActivationScheme(xmlConfig, "ActivationCppn");
        _activationScheme = ExperimentUtils.CreateActivationScheme(xmlConfig, "Activation");
        _complexityRegulationStr = XmlUtils.TryGetValueAsString(xmlConfig, "ComplexityRegulationStrategy");
        _complexityThreshold = XmlUtils.TryGetValueAsInt(xmlConfig, "ComplexityThreshold");
        _lengthCppnInput = XmlUtils.GetValueAsBool(xmlConfig, "LengthCppnInput");

        _eaParams = new NeatEvolutionAlgorithmParameters();
        _eaParams.SpecieCount = _specieCount;
        _neatGenomeParams = new NeatGenomeParameters();
    }

    public List<NeatGenome> LoadPopulation(XmlReader xr)
    {
        NeatGenomeFactory genomeFactory = (NeatGenomeFactory)CreateGenomeFactory();
        return NeatGenomeXmlIO.ReadCompleteGenomeList(xr, false, genomeFactory);
    }

    public void SavePopulation(XmlWriter xw, IList<NeatGenome> genomeList)
    {
        // Writing node IDs is not necessary for NEAT.
        NeatGenomeXmlIO.WriteComplete(xw, genomeList, true);
    }


    public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder()
    {
        //NOTE: HARCODED STUFF DUDE
        return CreateGenomeDecoder(12, _lengthCppnInput);
    }

    public IGenomeFactory<NeatGenome> CreateGenomeFactory()
    {
        return new CppnGenomeFactory(InputCount, OutputCount, GetCppnActivationFunctionLibrary(), _neatGenomeParams);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm()
    {
        return CreateEvolutionAlgorithm(_populationSize);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(int populationSize)
    {
        // Create a genome factory with our neat genome parameters object and the appropriate number of input and output neuron genes.
        IGenomeFactory<NeatGenome> genomeFactory = CreateGenomeFactory();

        // Create an initial population of randomly generated genomes.
        List<NeatGenome> genomeList = genomeFactory.CreateGenomeList(populationSize, 0);

        // Create evolution algorithm.
        return CreateEvolutionAlgorithm(genomeFactory, genomeList);
    }

    public NeatEvolutionAlgorithm<NeatGenome> CreateEvolutionAlgorithm(IGenomeFactory<NeatGenome> genomeFactory, List<NeatGenome> genomeList)
    {
        // Create distance metric. Mismatched genes have a fixed distance of 10; for matched genes the distance is their weight difference.
        IDistanceMetric distanceMetric = new ManhattanDistanceMetric(1.0, 0.0, 10.0);
        ISpeciationStrategy<NeatGenome> speciationStrategy = new KMeansClusteringStrategy<NeatGenome>(distanceMetric);

        // Create complexity regulation strategy.
        IComplexityRegulationStrategy complexityRegulationStrategy = ExperimentUtils.CreateComplexityRegulationStrategy(_complexityRegulationStr, _complexityThreshold);

        // Create the evolution algorithm.
        NeatEvolutionAlgorithm<NeatGenome> ea = new NeatEvolutionAlgorithm<NeatGenome>(_eaParams, speciationStrategy, complexityRegulationStrategy);

        // Create IBlackBox evaluator.
        BraidEvaluator evaluator = new BraidEvaluator(m_optimizer);

        // Create genome decoder. Decodes to a neural network packaged with an activation scheme that defines a fixed number of activations per evaluation.
        IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = CreateGenomeDecoder(10, _lengthCppnInput);

        // Create a genome list evaluator. This packages up the genome decoder with the genome evaluator.
        IGenomeListEvaluator<NeatGenome> innerEvaluator = new BraidListEvaluator<NeatGenome, IBlackBox>(genomeDecoder, evaluator, m_optimizer);

        // Wrap the list evaluator in a 'selective' evaluator that will only evaluate new genomes. That is, we skip re-evaluating any genomes
        // that were in the population in previous generations (elite genomes). This is determined by examining each genome's evaluation info object.
        IGenomeListEvaluator<NeatGenome> selectiveEvaluator = new SelectiveGenomeListEvaluator<NeatGenome>(
                                                                                innerEvaluator,
                                                                                SelectiveGenomeListEvaluator<NeatGenome>.CreatePredicate_OnceOnly());
        // Initialize the evolution algorithm.
        ea.Initialize(selectiveEvaluator, genomeFactory, genomeList);

        // Finished. Return the evolution algorithm
        return ea;
    }

    public int VisualFieldResolution
    {
        get { return 10; }
    }

    public bool LengthCppnInput
    {
        get { return _lengthCppnInput; }
    }


    #region Public Methods

    /// <summary>
    /// Creates a genome decoder. We split this code into a separate  method so that it can be re-used by the problem domain visualization code
    /// (it needs to decode genomes to phenomes in order to create a visualization).
    /// </summary>
    /// <param name="visualFieldResolution">The visual field's pixel resolution, e.g. 11 means 11x11 pixels.</param>
    /// <param name="lengthCppnInput">Indicates if the CPPNs being decoded have an extra input for specifying connection length.</param>
    public IGenomeDecoder<NeatGenome, IBlackBox> CreateGenomeDecoder(int visualFieldResolution, bool lengthCppnInput)
    {
        // Create two layer 'sandwich' substrate.
        int pixelCount = visualFieldResolution * visualFieldResolution;
        double pixelSize = 24;
        double originPixelXY = -1 + (pixelSize / 2.0);

        SubstrateNodeSet inputLayer = new SubstrateNodeSet(pixelCount);
        SubstrateNodeSet outputLayer = new SubstrateNodeSet(pixelCount);

        // Node IDs start at 1. (bias node is always zero).
        uint inputId = 1;
        uint outputId = (uint)(pixelCount + 1);
        double yReal = originPixelXY;

        for (int y = 0; y < visualFieldResolution; y++, yReal += pixelSize)
        {
            double xReal = originPixelXY;
            for (int x = 0; x < visualFieldResolution; x++, xReal += pixelSize, inputId++, outputId++)
            {
                inputLayer.NodeList.Add(new SubstrateNode(inputId, new double[] { xReal, yReal, -1.0 }));
                outputLayer.NodeList.Add(new SubstrateNode(outputId, new double[] { xReal, yReal, 1.0 }));
            }
        }

        List<SubstrateNodeSet> nodeSetList = new List<SubstrateNodeSet>(2);
        nodeSetList.Add(inputLayer);
        nodeSetList.Add(outputLayer);

        // Define connection mappings between layers/sets.
        List<NodeSetMapping> nodeSetMappingList = new List<NodeSetMapping>(1);
        nodeSetMappingList.Add(NodeSetMapping.Create(0, 1, (double?)null));

        // Construct substrate.
        Substrate substrate = new Substrate(nodeSetList, DefaultActivationFunctionLibrary.CreateLibraryNeat(SteepenedSigmoid.__DefaultInstance), 0, 0.2, 5, nodeSetMappingList);

        // Create genome decoder. Decodes to a neural network packaged with an activation scheme that defines a fixed number of activations per evaluation.
        IGenomeDecoder<NeatGenome, IBlackBox> genomeDecoder = new HyperNeatDecoder(substrate, _activationSchemeCppn, _activationScheme, lengthCppnInput);
        return genomeDecoder;
    }

    #endregion
    IActivationFunctionLibrary GetCppnActivationFunctionLibrary()
    {
        return DefaultActivationFunctionLibrary.CreateLibraryCppn();
    }
}