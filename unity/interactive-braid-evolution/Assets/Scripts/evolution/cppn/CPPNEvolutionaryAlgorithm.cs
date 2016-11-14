using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpNeat.Core;
using SharpNeat.DistanceMetrics;
using SharpNeat.EvolutionAlgorithms.ComplexityRegulation;
using SharpNeat.SpeciationStrategies;
using SharpNeat.Utility;
using System.Collections;

using System.IO;
using UnityEngine;

namespace SharpNeat.EvolutionAlgorithms
{
    public class CPPNEvolutionaryAlgorithm<TGenome> : AbstractGenerationalAlgorithm<TGenome>
        where TGenome : class, IGenome<TGenome>
    {
        protected NeatEvolutionAlgorithmParameters _eaParams;
        protected readonly NeatEvolutionAlgorithmParameters _eaParamsComplexifying;
        protected readonly NeatEvolutionAlgorithmParameters _eaParamsSimplifying;

        protected readonly ISpeciationStrategy<TGenome> _speciationStrategy;
        protected IList<Specie<TGenome>> _specieList;
        /// <summary>Index of the specie that contains _currentBestGenome.</summary>
        int _bestSpecieIdx;
        readonly FastRandom _rng = new FastRandom();
        protected readonly NeatAlgorithmStats _stats;

        protected ComplexityRegulationMode _complexityRegulationMode;
        protected readonly IComplexityRegulationStrategy _complexityRegulationStrategy;

        // P. SØRENSEN VARIABLES 
        public bool ReadyForNextGeneration;
        // END OF P. SØRENSEN VARIABLES


        public CPPNEvolutionaryAlgorithm()
        {
            _eaParams = new NeatEvolutionAlgorithmParameters();
            _eaParamsComplexifying = _eaParams;
            _eaParamsSimplifying = _eaParams.CreateSimplifyingParameters();
            _stats = new NeatAlgorithmStats(_eaParams);
            _speciationStrategy = new KMeansClusteringStrategy<TGenome>(new ManhattanDistanceMetric());

            _complexityRegulationMode = ComplexityRegulationMode.Complexifying;
            _complexityRegulationStrategy = new NullComplexityRegulationStrategy();
        }

        protected override IEnumerator PerformOneGeneration()
        {
            throw new NotImplementedException();
        }
    }
}
