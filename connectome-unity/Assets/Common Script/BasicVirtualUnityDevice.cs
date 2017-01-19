using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Enum;
using System;

namespace Connectome.Unity.Common
{
    //TODO create configuration device class 
    /// <summary>
    /// A Virtual device with a custome window. States are 'targetted' means holding target command and power. 
    /// There are two way to taget states: set force flag, or counter is greater than 0. 
    /// Force counter decreases after every non-forced state read. 
    /// </summary>
    public class BasicVirtualUnityDevice : MonoBehaviour, IEmotivDevice
    {
        #region Public Attributes
        /// <summary>
        /// Target Command to be read with targging 
        /// </summary>
        public EmotivCommandType TargetCommand = EmotivCommandType.PUSH;

        /// <summary>
        /// Target Power to be read with targging 
        /// </summary>
        public float TargetPower;

        /// <summary>
        /// Holds forcing target flag 
        /// </summary>
        public bool IsTargetForced;

        /// <summary>
        /// Holds number of states added when adding is issued. 
        /// </summary>
        public int ForceAddSize = 10; 

        /// <summary>
        /// Holds number of target statse read. 
        /// </summary>
        public int ForceCount = 0;

        public bool IsConnected
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region IEmotivDevice Events
        public event Action<string> OnConnectSucceed;
        public event Action<string> OnConnectFailed;
        public event Action<string> OnDisconnectSucceed;
        public event Action<string> OnDisconnectFailed;
        #endregion
        #region IEmotivDevice Public Methods
        public bool Connect(out string msg)
        {
            bool suc = true; 
            msg = "success";


            if (suc && OnConnectSucceed != null)
            {
                OnConnectSucceed(msg);
            }

            if(!suc && OnConnectFailed != null)
            {
                OnConnectFailed(msg);
            }
           
            return suc;
        }

        public bool Disconnect(out string msg)
        {
            bool suc = false; 
            Destroy(this.gameObject);

            suc = true; 

            //TODO will this get called??
            msg = "success";
            if (suc && OnDisconnectSucceed != null)
            {
                OnDisconnectSucceed(msg);
            }

            if(!suc && OnDisconnectFailed != null)
            {
                OnDisconnectFailed(msg);
            }
            return suc; 
        }

		public void Disconnect()
		{
			string msg = null;
			this.Disconnect (out msg);
		}

        /// <summary>
        /// Reads when either target is forced or force counter is greater than 1. 
        /// </summary>
        /// <returns></returns>
        public IEmotivState Read(long time)
        {
            if (IsTargetForced)
            {
                ForceCount = ForceCount == 0 ? 1 : ForceCount; 
            }
            if(ForceCount > 0)
            {
                ForceCount--;
                return new EmotivState(TargetCommand, TargetPower, time);
            }
            else
            {
                return new EmotivState(EmotivCommandType.NEUTRAL, time);
            }
        }
        #endregion
        #region UI Events 
        /// <summary>
        /// Set power from silder. 
        /// </summary>
        /// <param name="s"></param>
        public void SetTargetPower(Slider s)
        {
            TargetPower = s.value;
        }

        /// <summary>
        /// Update power text  
        /// </summary>
        /// <param name="s"></param>
        public void UpdateSliderTextValue(Text text)
        {
            text.text = TargetPower.ToString("0.00");
        }

        /// <summary>
        /// Set command from dropdown 
        /// </summary>
        /// <param name="dd"></param>
        public void SetTargetCommand(Dropdown dd)
        {
            TargetCommand = (EmotivCommandType)dd.value;
        }

        /// <summary>
        /// Set forcing target
        /// </summary>
        /// <param name="t"></param>
        public void SetForceTarget(Toggle t)
        {
            IsTargetForced = t.isOn;
        }

        /// <summary>
        /// Adds force counts when button is clicked. 
        /// </summary>
        public void AddForcedState()
        {
           ForceCount += ForceAddSize; 
        }
        #endregion
    }
}
