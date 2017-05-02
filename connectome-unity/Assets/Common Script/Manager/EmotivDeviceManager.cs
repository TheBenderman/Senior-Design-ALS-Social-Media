using Connectome.Emotiv.Interface;
using Connectome.Unity.Expection;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Core.Interface;

namespace Connectome.Unity.Manager
{
    /// <summary>
    /// Holds Device, and Reader as well as running interpetation. 
    /// </summary>
    public class EmotivDeviceManager : DeviceManager<IEmotivState>
    {
        #region Public Inspector Attributes 
        [Header("Emotiv Requirements")]
        /// <summary>
        /// Hold Device
        /// </summary>
        public EmotivDevicePlugin DevicePlugin;
        /// <summary>
        /// Holds Redaer 
        /// </summary>
        public EmotivReaderPlugin ReaderPlugin;

        [Header("Data Process Requirements")]
        /// <summary>
        /// Holds Samplers with creates samples to interpret
        /// </summary>
        public EmotivSampler Sampler;

        /// <summary>
        /// Holds data sample interpreters 
        /// </summary>
        public EmotivInterpreter[] Interpreters;

        #endregion
        #region DeviceManager Overrides
        protected override IConnectomeDevice<IEmotivState> GetDevice()
        {
            DevicePlugin.Setup();
            return DevicePlugin;
        }

        protected override IConnectomeReader<IEmotivState> GetReader()
        {
            ReaderPlugin.SetUp(DevicePlugin);
            return ReaderPlugin;
        }

        protected override DataSampler<IEmotivState> GetSampler()
        {
            return Sampler;
        }

        protected override DataInterpreter<IEmotivState>[] GetInterpreters()
        {
            return Interpreters;
        }

        /// <summary>
        /// Convert wireless signal strength based on Epoc Emotv device way 
        /// </summary>
        /// <param name="wirelessSignal"></param>
        /// <returns></returns>
        protected override WirelessSignalStrengthLevel ConvertWirelessStrength(int wirelessSignal)
        {
            return (WirelessSignalStrengthLevel)wirelessSignal;
            /* ### Dummy way 
            switch (wirelessSignal)
            {
                case 0:
                    return WirelessSignalStrengthLevel.NO_SIGNAL;
                case 1:
                    return WirelessSignalStrengthLevel.BAD_SIGNAL;
                case 2:
                    return WirelessSignalStrengthLevel.GOOD_SIGNAL;
            }
            */
        }

        /// <summary>
        /// Convert battery level based on Epoc Emotv device way 
        /// </summary>
        /// <param name="batteryLevel"></param>
        /// <returns></returns>
        protected override BatteryLevel ConvertBatteryLevel(int batteryLevel)
        {
            //TODO not sure what value is recieved from emotiv
            if (batteryLevel <= 33)
            {
                return BatteryLevel.LOW;
            }
            else if (batteryLevel <= 66)
            {
                return BatteryLevel.MEDIUM;
            }
            else
            {
                return BatteryLevel.FULL;
            }
        }

        /// <summary>
        /// Convert input rate level based on Epoc Emotv device way 
        /// </summary>
        /// <param name="inputRate"></param>
        /// <returns></returns>
        protected override InputRateLevel ConvertInputRateLevel(int inputRate)
        {
            //Debug.Log(inputRate)
            //Not sure whats the stable input rate 
            if (inputRate <= 2)
            {
                return InputRateLevel.UNUSABLE;
            }
            else if (inputRate <= 5)
            {
                return InputRateLevel.BAD;
            }
            else
            {
                return InputRateLevel.GOOD;
            }
        }
        #endregion
        #region Validation 
        /// <summary>
        /// Validate required component 
        /// </summary>
        private void OnValidate()
        {
            ValidateDevice();
            ValidateReader();
            ValidateInterpreters();
        }

        /// <summary>
        ///  warns when device is null 
        /// </summary>
        private void ValidateDevice()
        {
            if (DevicePlugin == null)
            {
                Debug.LogWarning("Device is null", this);
            }
        }

        /// <summary>
        ///  warns when reader is null 
        /// </summary>
        private void ValidateReader()
        {
            if (ReaderPlugin == null)
            {
                Debug.LogWarning("Reader is null", this);
            }
        }

        /// <summary>
        ///  insure no interpreter in null
        /// </summary>
        private void ValidateInterpreters()
        {
            for (int i = 0; i < Interpreters.Length; i++)
            {
                if (Interpreters[i] == null)
                    Debug.LogError("Interpreter at index " + i + " is null");
            }
        }
        #endregion
    }
}

