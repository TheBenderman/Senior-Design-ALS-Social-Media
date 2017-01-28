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
public class ProcessorPlugin<T> : MonoBehaviour
{
    #region Protected Attributes
    /// <summary>
    /// The Attached plugin
    /// </summary>
    protected IProcessable<T> p;
    #endregion
    #region Public Methods
    /// <summary>
    /// Get the Plugin
    /// </summary>
    /// <returns></returns>
    public virtual IProcessable<T> GetPlugin()
    {
        return p;
    }

    /// <summary>
    /// Set the plugin
    /// </summary>
    /// <param name="p"></param>
    public virtual void SetPlugin(IProcessable<T> p)
    {
        this.p = p;
    }
    #endregion
}
