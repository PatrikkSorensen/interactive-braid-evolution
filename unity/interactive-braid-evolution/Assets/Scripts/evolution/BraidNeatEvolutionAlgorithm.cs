/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2006, 2009-2010 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * SharpNEAT is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with SharpNEAT.  If not, see <http://www.gnu.org/licenses/>.
 */

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
    public class BraidNeatEvolutionAlgorithm<TGenome> : NeatEvolutionAlgorithm<TGenome>
        where TGenome : class, IGenome<TGenome>
    {

        Logger myLogger;
        String logTag = "BraidNeatEvolutionAlgorithm: ";

        #region Constructors
        public BraidNeatEvolutionAlgorithm() : base() { }
        

        public BraidNeatEvolutionAlgorithm(NeatEvolutionAlgorithmParameters eaParams,
                                      ISpeciationStrategy<TGenome> speciationStrategy,
                                      IComplexityRegulationStrategy complexityRegulationStrategy) : base(eaParams,
                                      speciationStrategy,
                                      complexityRegulationStrategy)
        {
            myLogger = new Logger(new MyLogger());
            myLogger.Log(logTag, "Initialized");
        }

        #endregion

        protected override IEnumerator PerformOneGeneration()
        {
            myLogger = new Logger(new MyLogger());
            myLogger.Log(logTag, "Performing One Generation."); 
            
            // Calculate statistics for each specie (mean fitness, target size, number of offspring to produce etc.)
            int offspringCount;
            SpecieStats[] specieStatsArr = CalcSpecieStats(out offspringCount);

            // Create offspring.
            List<TGenome> offspringList = CreateOffspring(specieStatsArr, offspringCount);

            // Trim species back to their elite genomes.
            bool emptySpeciesFlag = TrimSpeciesBackToElite(specieStatsArr);

            // Rebuild _genomeList. It will now contain just the elite genomes.
            RebuildGenomeList();

            // Append offspring genomes to the elite genomes in _genomeList. We do this before calling the
            // _genomeListEvaluator.Evaluate because some evaluation schemes re-evaluate the elite genomes 
            // (otherwise we could just evaluate offspringList).
            _genomeList.AddRange(offspringList);

            /**********************  IEC SPECIFIC CODE BEGINS HERE ******************************/ 
            while (!ReadyForNextGeneration)
            {
                myLogger.Log(logTag, "waiting for input"); 
                yield return new WaitForSeconds(2.0f);
            }

            /**********************  IEC SPECIFIC CODE ENDS HERE   ******************************/
            yield return Coroutiner.StartCoroutine( _genomeListEvaluator.Evaluate(_genomeList));

            // Integrate offspring into species.
            if(emptySpeciesFlag)
            {   
                // We have one or more terminated species. Therefore we need to fully re-speciate all genomes to divide them
                // evenly between the required number of species.

                // Clear all genomes from species (we still have the elite genomes in _genomeList).
                ClearAllSpecies();

                // Speciate genomeList.
                _speciationStrategy.SpeciateGenomes(_genomeList, _specieList);
            }
            else
            {
                // Integrate offspring into the existing species. 
                _speciationStrategy.SpeciateOffspring(offspringList, _specieList);            
            }
            //Debug.Assert(!TestForEmptySpecies(_specieList), "Speciation resulted in one or more empty species.");

            // Sort the genomes in each specie. Fittest first (secondary sort - youngest first).
            SortSpecieGenomes();
             
            // Update stats and store reference to best genome.
            UpdateBestGenome();
            UpdateStats();

            // Determine the complexity regulation mode and switch over to the appropriate set of evolution
            // algorithm parameters. Also notify the genome factory to allow it to modify how it creates genomes
            // (e.g. reduce or disable additive mutations).
            _complexityRegulationMode = _complexityRegulationStrategy.DetermineMode(_stats);
            _genomeFactory.SearchMode = (int)_complexityRegulationMode;
            switch(_complexityRegulationMode)
            {
                case ComplexityRegulationMode.Complexifying:
                    _eaParams = _eaParamsComplexifying;
                    break;
                case ComplexityRegulationMode.Simplifying:
                    _eaParams = _eaParamsSimplifying;
                    break;
            }
        }
    }
}
