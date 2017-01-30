using Connectome.Core.Implementation;
using Connectome.Core.Interface;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The wrapper for the Refresh Processee
/// </summary>
public class RefreshProcesseePlugin : ProcesseePlugin<ITimeline<IEmotivState>>
{
    public long RefreshInterval;
    public EmotivCommandType TargetCommand;

    public float RefreshThreshhold;

    #region Unity Methods
    /// <summary>
    /// The processee is instantiated when the game object is instantiated in the scene
    /// </summary>
    public override void Init()
    {
        RefreshProcessee refresh = new RefreshProcessee();

        refresh.RefreshInterval = RefreshInterval;
        refresh.TargetCommand = TargetCommand;
        refresh.ThreashHold = RefreshThreshhold;

        Process = refresh; 
    }
    #endregion
}
