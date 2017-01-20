using UnityEngine;
using System.Collections;

namespace Connectome.Unity.Demo
{
    /// <summary>
    /// Forces yes and no's 
    /// leave this alone. 
    /// </summary>
    public class InputContainer : MonoBehaviour
    {
        #region Public attributes 
        /// <summary>
        /// Instence
        /// </summary>
        public InputGenerator Instence
        {
            get
            {
                return privateInstence ?? (privateInstence = new InputGenerator());
            }
        }
        #endregion
        #region Private attributes 
        private InputGenerator privateInstence;
        #endregion
        #region Public methods 
        /// <summary>
        /// Generate Yes 
        /// </summary>
        public void Yes()
        {
            if (Instence.OnYes != null)
                Instence.OnYes();
        }

        /// <summary>
        /// Generate No (Not used anywhere)
        /// </summary>
        public void No()
        {
            if (Instence.OnNo != null)
                Instence.OnNo();
        }
        #endregion 
    }
}
