using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using System.Linq;
using Connectome.Emotiv.Enum;

/// <summary>
/// Calculates a sample based on rate (percentage) target command with in the sample. 
/// </summary>
public class CommandTypeCalculator : EmotivRateCalculator
{
    /// <summary>
    /// Enables and disabled rate reduction for samples below minimum size. 
    /// <see cref="EmotivCalculatorConfiguration.MinSampleSize"/>
    /// </summary>
    public bool Reduction;

    /// <summary>
    /// Default config
    /// </summary>
    public EmotivCalculatorConfiguration DefaultConfig; 

    public override float Calculate(IEnumerable<IEmotivState> sample, EmotivCalculatorConfiguration config=null)
    {
        if(config == null)
        {
            config = DefaultConfig; 
        }

        float Rate = ((float)sample.Where(s => s.Command == config.TargetCommand).Count()) / sample.Count();

        if (Reduction && (sample.Count() <= config.MinSampleSize))
        {
            Rate *= ((float)sample.Count()) / config.MinSampleSize;
        }

        return Rate; 
    }
}
