using Connectome.Core.Implementation;
using Connectome.Core.Int;
using Connectome.Core.Interface;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Wrapper for the Timeline Processor Plugin
/// </summary>
public class TimelineProcessorPlugin : ProcessorPlugin<SelectionManager>
{
    #region Public Attributes
    /// <summary>
    /// The list of Game Objects holding the Processes that this Processor Keeps track of
    /// </summary>
    public GameObject[] ProcesseeObjects;
    #endregion
    #region Protected Attributes
    /// <summary>
    /// The List of Processes that this Processor keeps track of. Used to instantiate the Core-side Processor
    /// </summary>
    protected IProcessable<ITimeline<IEmotivState>>[] Children;
    #endregion
    #region Unity Methods
    /// <summary>
    /// When this game object is created, look through the list of Children in order to instantiate the plugin.
    /// </summary>
    private void Start()
    {
        Children = new IProcessable<ITimeline<IEmotivState>>[ProcesseeObjects.Length];
        for (int i = 0; i < ProcesseeObjects.Length; i++)
        {
            Children[i] = ProcesseeObjects[i].GetComponent<ProcessorPlugin<ITimeline<IEmotivState>>>().GetPlugin();
        }
        SetPlugin(new TimelineProcessor<SelectionManager>(new Timeline<IEmotivState>(), Children));
    }
    #endregion
}
