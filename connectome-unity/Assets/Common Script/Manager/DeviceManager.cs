using Connectome.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Connectome.Unity.UI;

namespace Connectome.Unity.Manager
{
    /// <summary>
    /// DeviceManager managers a device and a reader by making sure they run, and display proper notifications when any stops working. 
    /// It also controls Interprepter that recieve a data sample read from device by reader. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DeviceManager<T> : MonoBehaviour
    {
        #region Inspecter Attributes 
        /// <summary>
        /// Allows DeviceManager to auto start 
        /// </summary>
        public bool AutoSetup;

        /// <summary>
        /// Holds StatudBar which displays InputRate, BatteryLevel, and WirelessSignal. 
        /// </summary>
        public DeviceStatusbar StatusBar;
        #endregion
        #region Private Attributes 
        /// <summary>
        /// Holds device 
        /// </summary>
        private IConnectomeDevice<T> Device;

        /// <summary>
        /// Holds reader  
        /// </summary>
        private IConnectomeReader<T> Reader;

        /// <summary>
        /// Holds read data and samples it for interpeters
        /// </summary>
        private DataSampler<T> Sampler;

        /// <summary>
        /// Holds interpreters to be invoked
        /// </summary>
        private DataInterpreter<T>[] Interpeters;

        /// <summary>
        /// Counts total states read 
        /// </summary>
        private int TotalStatesRead;

        /// <summary>
        /// Holds previously Recognised total states. Used to calculate input rate.
        /// </summary>
        private int PreviousInputRate;
        #endregion
        #region Virtual Methods 
        /// <summary>
        /// Sets up Device, Reader, and Sampler then start interpreters coroutine 
        /// </summary>
        public virtual void Setup()
        {
            Device = GetDevice();
            Reader = GetReader();
            Sampler = GetSampler();
            Interpeters = GetInterpreters();

            Reader.OnRead += Sampler.Register;
            Reader.OnRead += (s) => TotalStatesRead++;

            Reader.StartReading();

            StartCoroutine(InterpetationProcess());
            StartCoroutine(UpdateStatusBar());
        }
        #endregion
        #region Unity Methods 
        /// <summary>
        /// Auto runs Manager if  AutoSetup is checked  
        /// </summary>
        private void Start()
        {
            if (AutoSetup)
            {
                Setup();
            }
        }

        /// <summary>
        /// Insures reader is disabled
        /// </summary>
        void OnApplicationQuit()
        {
            if (Reader != null && Reader.IsReading)
            {
                Reader.StopReading();
            }
        }
        #endregion
        #region Coroutine 
        /// <summary>
        /// Coroutine that Interpret every intepreter with an allocated sample. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator InterpetationProcess()
        {
            while (true)
            {
                yield return new WaitForSeconds(0); ///needed to execute otherwise it'll be stuck. 
                IEnumerable<T> Sample = Sampler.GetSample();

                foreach (var interpeter in Interpeters)
                {
                    interpeter.Interpeter(Sample);
                }
            }
        }

        /// <summary>
        /// Updates DeviceStatusBar at a lower rate than Interpreters 
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateStatusBar()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.25f);
                if (StatusBar != null)
                {
                    StatusBar.UpdateWirelessSignalStrength(ConvertWirelessStrength(Device.WirelessSignalStrength));
                    StatusBar.UpdateBatteryLevel(ConvertBatteryLevel(Device.BatteryLevel));

                    StatusBar.UpdateInputRate(ConvertInputRateLevel(TotalStatesRead - PreviousInputRate));
                }
                    PreviousInputRate = TotalStatesRead;
                
            }
        }

        #endregion
        #region Abstarct Methods 
        /// <summary>
        /// Gets a Device 
        /// </summary>
        /// <returns>An initilized device</returns>
        protected abstract IConnectomeDevice<T> GetDevice();
        /// <summary>
        /// Gets a Reader 
        /// </summary>
        /// <returns>An initilized reader</returns>
        protected abstract IConnectomeReader<T> GetReader();
        /// <summary>
        /// Gets a DataSampler 
        /// </summary>
        /// <returns>An initilized DataSampler</returns>
        protected abstract DataSampler<T> GetSampler();
        /// <summary>
        /// Gets Interpreters  
        /// </summary>
        /// <returns>Initilized set fo interpreters</returns>
        protected abstract DataInterpreter<T>[] GetInterpreters();

        /// <summary>
        /// Defines how to covert Device's wireless signal  
        /// </summary>
        /// <param name="wirelessSignal">Signal value read from device</param>
        /// <returns>A converted signal value</returns>
        protected abstract WirelessSignalStrengthLevel ConvertWirelessStrength(int wirelessSignal);

        /// <summary>
        /// Defines how to covert Device's battery level
        /// </summary>
        /// <param name="batteryLevel">Battery level read from device</param>
        /// <returns></returns>
        protected abstract BatteryLevel ConvertBatteryLevel(int batteryLevel);

        /// <summary>
        /// Defines how to covert Device's inpur rate level
        /// </summary>
        /// <param name="inputRate">Raw input value</param>
        /// <returns>A converted input level</returns>
        protected abstract InputRateLevel ConvertInputRateLevel(int inputRate);
        #endregion
        #region Validate 
        /// <summary>
        /// Checks of AutoSetup is enabled. 
        /// </summary>
        private void OnValidate()
        {
            if (AutoSetup)
            {
                Debug.LogWarning("Remember to disable AutoRun before building.");
            }
        }
        #endregion
    }
}