using System.Collections;
using System.Collections.Generic;
using Connectome.Core.Interface;
using Connectome.Unity.Common;
using UnityEngine;
using UnityEngine.Events;
using Connectome.Core.Template;
/// <summary>
/// A wrapper for the Processee
/// </summary>
/// <typeparam name="T"></typeparam>
public class ProcesseePlugin<T> : ProcessPlugin<T>
{
    #region Public Attributes
    public UnityEvent OnExecute;
    #endregion
    #region Private Attributes
    private bool EventLoaded = false;
    #endregion
    #region Overridden Methods
    public override IProcessable<T> GetPlugin()
    {
        if (Process is Processee<T> && !EventLoaded)
        {
            ((Processee<T>)Process).OnExecute += (m) => { OnExecute.Invoke(); };
            EventLoaded = true;
        }
        return base.Process; 
    }
    #endregion
}
