using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivWrapperInterface;
using System;
using EmotivWrapper;
using UnityEngine.UI;

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
        public EmotivStateType TargetCommand = EmotivStateType.PUSH;

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
        #endregion
        #region IEmotivDevice Interface
        public bool Connect(out string errorMsg)
        {
            errorMsg = "success";
            return true;
        }

        public bool ConnectionSetUp(out string errorMsg)
        {
            errorMsg = "success";
            return true;
        }

        public void Disconnect()
        {
            Destroy(this.gameObject); 
        }

        public bool DisconnectionSetUp(){ return true; }

        /// <summary>
        /// Reads when either target is forced or force counter is greater than 1. 
        /// </summary>
        /// <returns></returns>
        public IEmotivState Read()
        {
            if (IsTargetForced)
            {
                ForceCount = ForceCount == 0 ? 1 : ForceCount; 
            }
            if(ForceCount > 0)
            {
                ForceCount--;
                return new EmotivState() { power = TargetPower, command = TargetCommand };
            }
            else
            {
                return new EmotivState() { power = 0, command = EmotivStateType.NEUTRAL };
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
            TargetCommand = (EmotivStateType)dd.value;
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
