using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Connectome.Core.Interface
{
    /// <summary>
    /// A Reader is able to read states on it's own invoking events when appropriate. 
    /// </summary>
    /// <typeparam name="T"> State type that is read</typeparam>
    public interface IConnectomeReader<T>
    {
        #region Set Get Properties
        /// <summary>
        /// Current plugged device. 
        /// </summary>
        IConnectomeDevice<T> Device { set; get; }
        #endregion
        #region Get Properties
        /// <summary>
        /// True when device is reading or ready to read, otherwise, false. 
        /// </summary>
        bool IsReading { get; }
        #endregion
        #region Events
        /// <summary>
        /// Invoked on every state read from plugged device.
        /// </summary>
        event Action<T> OnRead;

        /// <summary>
        /// Gets invoked before reading states
        /// </summary>
        event Action OnStart;

        /// <summary>
        /// Gets invoked after reader stops reading. 
        /// </summary>
        event Action<string> OnStop;

        /// <summary>
        /// Handles thrown exception
        /// </summary>
        event Action<Exception> ExceptionHandler;
        #endregion
        #region Methods
        /// <summary>
        /// Starts Reading from plugged device.
        /// </summary>
        void StartReading();

        /// <summary>
        /// Stops Reading from plugged device.
        /// </summary>
        void StopReading();

        /// <summary>
        /// Sets and starts reading from device. 
        /// </summary>
        /// <param name="Device"></param>
        void PlugDevice(IConnectomeDevice<T> Device);
        #endregion 
    }
}
