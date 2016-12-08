using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpNeat.Core;
using System.Collections;
using UnityEngine;

namespace SharpNEAT.core
{
    class BraidListEvaluator<TGenome, TPhenome> : IGenomeListEvaluator<TGenome>
        where TGenome : class, IGenome<TGenome>
        where TPhenome : class
    {
        IGenomeDecoder<TGenome, TPhenome> m_genomeDecoder;
        IPhenomeEvaluator<TPhenome> m_phenomeEvaluator;

        Optimizer m_optimizer;

        public BraidListEvaluator(IGenomeDecoder<TGenome, TPhenome> genomeDecoder,
                                         IPhenomeEvaluator<TPhenome> phenomeEvaluator,
                                          Optimizer opt)
        {
            m_genomeDecoder = genomeDecoder;
            m_phenomeEvaluator = phenomeEvaluator;
            m_optimizer = opt; 
        }

        public ulong EvaluationCount
        {
            get { return m_phenomeEvaluator.EvaluationCount; }
        }

        public bool StopConditionSatisfied
        {
            get { return m_phenomeEvaluator.StopConditionSatisfied; }
        }

        public IEnumerator Evaluate(IList<TGenome> genomeList)
        {
            yield return Coroutiner.StartCoroutine(evaluateList(genomeList));
        }

        private IEnumerator evaluateList(IList<TGenome> genomeList)
        {
            Debug.Log("---------------------- Evaluating List of genomes ----------------------");

            Dictionary<TGenome, TPhenome> dict = new Dictionary<TGenome, TPhenome>();
            Dictionary<TGenome, FitnessInfo[]> fitnessDict = new Dictionary<TGenome, FitnessInfo[]>();
            for (int i = 0; i < m_optimizer.Trials; i++)
            {
                m_phenomeEvaluator.Reset();
                dict = new Dictionary<TGenome, TPhenome>();
                foreach (TGenome genome in genomeList)
                {
                    TPhenome phenome = m_genomeDecoder.Decode(genome);
                    if (i == 0)
                        fitnessDict.Add(genome, new FitnessInfo[m_optimizer.Trials]);

                    dict.Add(genome, phenome);
                    Coroutiner.StartCoroutine(m_phenomeEvaluator.Evaluate(phenome));
                }

                while (!BraidSimulationManager.HasControllersCreatedData())
                {
                    Debug.Log("Waiting..."); 
                    yield return new WaitForSeconds(0.2f);
                }

                ModelMessenger messenger = GameObject.FindObjectOfType<ModelMessenger>();
                messenger.SendMessageToGH();

                while (!BraidSimulationManager.HasControllersEvaluated())
                    yield return new WaitForSeconds(0.2f);

                BraidSimulationManager.AdvanceGeneration(); 

                Debug.Log("Getting fitness values...");
                foreach (TGenome genome in dict.Keys)
                {

                    TPhenome phenome = dict[genome];
                    if (phenome != null)
                    {

                        FitnessInfo fitnessInfo = m_phenomeEvaluator.GetLastFitness(phenome);

                        fitnessDict[genome][i] = fitnessInfo;
                    }
                }
                Debug.Log("Done getting fitness values...");
            }
            

            foreach (TGenome genome in dict.Keys)
            {
                TPhenome phenome = dict[genome];
                if (phenome != null)
                {
                    double fitness = 0;

                    for (int i = 0; i < m_optimizer.Trials; i++)
                        fitness += fitnessDict[genome][i]._fitness;

                    fitness /= m_optimizer.Trials; // Averaged fitness

                    genome.EvaluationInfo.SetFitness(fitness);
                    genome.EvaluationInfo.AuxFitnessArr = fitnessDict[genome][0]._auxFitnessArr;
                }
            }

            Debug.Log("---------------------- End of list evaluation ----------------------");
            BraidSimulationManager.evaluationsMade = 0; 
        }

        public void Reset()
        {
             m_phenomeEvaluator.Reset();
        }
    }
}
