using Connectome.Core.Implementation;
using Connectome.Core.Int;
using Connectome.Core.Interface;
using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A Wrapper for the Click Processee
/// </summary>
public class ClickProcesseePlugin : ProcesseePlugin<ITimeline<IEmotivState>> {

    #region Unity Methods
    /// <summary>
    /// The Processee is instantiated when the gameobject is created in the scene.
    /// </summary>
    void Start () {
        p = new ClickProcessee();
    }
    #endregion
}
