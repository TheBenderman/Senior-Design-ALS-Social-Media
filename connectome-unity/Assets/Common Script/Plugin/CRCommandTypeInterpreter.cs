using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// ClickRefreshInterperter that interprets float value by percentage of targetted command in sample. 
    /// <see cref="ClickRefreshInterperter"/>
    /// </summary>
    public class CRCommandTypeInterpreter : ClickRefreshInterperter
    {
        /// <summary>
        /// Interprets and return a rating to decide click and/or refresh 
        /// </summary>
        /// <param name="sample">sample to generate calcuation from</param>
        /// <returns>Rate of targetted command in sample</returns>
        protected override float GetRate(IEnumerable<IEmotivState> sample)
        {
            return ((float)sample.Where(s => s.Command == TargetCommand).Count()) / sample.Count();
        }
    }
}
