﻿using UnityEngine;
using System.Collections;
using SharpNeat.Core;
using SharpNeat.Phenomes;
using System.Collections.Generic;
using System;

public class BraidEvaluator : IPhenomeEvaluator<IBlackBox>
{
    ulong _evalCount;
    bool _stopConditionSatisfied;
    Optimizer optimizer;
    FitnessInfo fitness;

    Dictionary<IBlackBox, FitnessInfo> dict = new Dictionary<IBlackBox, FitnessInfo>();

    public ulong EvaluationCount
    {
        get { return _evalCount; }
    }

    public bool StopConditionSatisfied
    {
        get { return _stopConditionSatisfied; }
    }

    public BraidEvaluator(Optimizer se)
    {
        this.optimizer = se;
    }

    public IEnumerator Evaluate(IBlackBox box)
    {
        if (optimizer != null)
        {
            optimizer.Evaluate(box);

            while (BraidSelector.ShouldEvaluate())
                yield return new WaitForSeconds(0.1f);

            optimizer.StopEvaluation(box);
            float fit = optimizer.GetFitness(box);
            FitnessInfo fitness = new FitnessInfo(fit, fit);
            dict.Add(box, fitness);
            BraidSimulationManager.evaluationsMade++; 
        }
    }

    public void Reset()
    {
        this.fitness = FitnessInfo.Zero;
        dict = new Dictionary<IBlackBox, FitnessInfo>();
    }

    public FitnessInfo GetLastFitness()
    {

        return this.fitness;
    }


    public FitnessInfo GetLastFitness(IBlackBox phenome)
    {
        if (dict.ContainsKey(phenome))
        {
            FitnessInfo fit = dict[phenome];
            dict.Remove(phenome);

            return fit;
        }

        return FitnessInfo.Zero;
    }
}
