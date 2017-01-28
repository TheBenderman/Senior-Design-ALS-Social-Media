using System.Collections;
using System.Collections.Generic;
using Connectome.Core.Interface;
using Connectome.Unity.Common;
using UnityEngine;
using UnityEngine.Events;
using Connectome.Core.Template;

public class ProcesseePlugin : ProcessorPlugin {
    public UnityEvent OnExecute;
    private bool EventLoaded = false;
    
    public override IProcessable<SelectionManager> GetPlugin()
    {
        if (p is Processee<SelectionManager> && !EventLoaded)
        {
            ((Processee<SelectionManager>)p).OnExecute += (m) => { OnExecute.Invoke(); };
            EventLoaded = true;
        }
        return base.p; 
    }


}
