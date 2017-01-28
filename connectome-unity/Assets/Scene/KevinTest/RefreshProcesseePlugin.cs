using Connectome.Core.Implementation;
using Connectome.Core.Interface;
using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The wrapper for the Refresh Processee
/// </summary>
public class RefreshProcesseePlugin : ProcesseePlugin<ITimeline<IEmotivState>> {
    #region Unity Methods
    /// <summary>
    /// The processee is instantiated when the game object is instantiated in the scene
    /// </summary>
    void Start () {
        p = new RefreshProcessee();	
	}
    #endregion
}
