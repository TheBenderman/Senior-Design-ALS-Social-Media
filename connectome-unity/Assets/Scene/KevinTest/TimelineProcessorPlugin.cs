using Connectome.Core.Common;
using Connectome.Core.Implementation;
using Connectome.Core.Interface;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Connectome.Unity.Common
{
    /// <summary>
    /// Wrapper for the Timeline Processor Plugin
    /// </summary>
    public class TimelineProcessorPlugin : ProcessPlugin<SelectionManager>
    {
        #region Public Attributes
        /// <summary>
        /// The list of Game Objects holding the Processes that this Processor Keeps track of
        /// </summary>
        public GameObject[] ProcesseeObjects;

        public TimelineDisplayProcesee process;

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
        void Start()
        {
        }
        #endregion

        public override void Init()
        {
            Children = new IProcessable<ITimeline<IEmotivState>>[ProcesseeObjects.Length+1];

            int i;
            for (i = 0; i < ProcesseeObjects.Length; i++)
            {
                var proc = ProcesseeObjects[i].GetComponent<ProcessPlugin<ITimeline<IEmotivState>>>();

                proc.Init(); 

                Children[i] = proc.GetPlugin();
            }

            Children[i] = process; 

           t = new Timeline<IEmotivState>(); 

            TimelineProcessor<SelectionManager> timelineProc = new TimelineProcessor<SelectionManager>(t, Children);
            timelineProc.OnChildExecute += (a, b) => { Debug.Log("My child did it");  };

            //while (UserSettings.Reader == null); 
            Debug.Log("need device");
            //Get Device and attack TimelineProcessor to it's OnRead
            UserSettings.Reader.OnRead += timelineProc.Track;

            SetPlugin(timelineProc);

            StartCoroutine(GetSize());
        }

        Timeline<IEmotivState> t; 
        public int Size;

        public bool Reading = false;
        public bool Device = false;

        public IEnumerator GetSize()
        {
            while(true)
            {
                yield return new WaitForSeconds(0.1f);
                Size =(int) t.Duration;
                Reading = UserSettings.Reader.IsReading;
                Device = UserSettings.Device.IsConnected; 
            }
        }

    }
}
