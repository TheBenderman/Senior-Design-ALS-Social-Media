using Connectome.Core.Interface;
using Connectome.Unity.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// A wrapper for Processors
/// </summary>
/// <typeparam name="T"></typeparam>
public class ProcessPlugin<T> : MonoBehaviour
{
    #region Protected Attributes
    /// <summary>
    /// The Attached plugin
    /// </summary>
    protected IProcessable<T> Process;
    #endregion
    #region Public Methods
    /// <summary>
    /// Get the Plugin
    /// </summary>
    /// <returns></returns>
    public virtual IProcessable<T> GetPlugin()
    {
        return Process;
    }

    /// <summary>
    /// Set the plugin
    /// </summary>
    /// <param name="p"></param>
    public virtual void SetPlugin(IProcessable<T> p)
    {
        this.Process = p;
    }

    /// <summary>
    /// Initlizes Process 
    /// </summary>
    public virtual void Init()
    {

    }
    #endregion
}
