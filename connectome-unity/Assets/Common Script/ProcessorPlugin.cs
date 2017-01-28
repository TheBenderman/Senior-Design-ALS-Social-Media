using Connectome.Core.Interface;
using Connectome.Unity.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProcessorPlugin : MonoBehaviour
{
    protected IProcessable<SelectionManager> p; 
    #region PublicAttributes
    public virtual IProcessable<SelectionManager> GetPlugin()
    {
        return p;
    }

    public virtual void SetPlugin(IProcessable<SelectionManager> p)
    {
        this.p = p;
    }
    #endregion
}
