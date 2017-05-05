using Connectome.Emotiv.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds emotiv claculator configrations 
/// </summary>
public class EmotivCalculatorConfiguration : MonoBehaviour
{
    /// <summary>
    /// Targetted command 
    /// </summary>
    public EmotivCommandType TargetCommand;

    /// <summary>
    /// Minimum sample size 
    /// </summary>
    public float MinSampleSize; 
}
