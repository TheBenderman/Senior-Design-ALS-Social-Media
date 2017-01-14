using System;

namespace Connectome.Unity.Demo
{
    /// <summary>
    /// Holds two delegates 
    /// </summary>
    public class InputGenerator
    {
        #region Public attributes; 
        /// <summary>
        /// Holds 'yes' delegates 
        /// </summary>
        public Action OnYes;
        /// <summary>
        /// Holds 'no' delegates 
        /// </summary>
        public Action OnNo;
        #endregion
    }
}
