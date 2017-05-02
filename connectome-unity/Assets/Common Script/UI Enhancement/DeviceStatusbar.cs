using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Displays Device's states as Images 
    /// </summary>
    public class DeviceStatusbar : MonoBehaviour
    {
        [Header("Target Components")]
        public Image WirelessImage;
        public Image BatteryImage;
        public Image InputRateImage;

        [Header("Status Sprites (Ascending Order)")]
        public Sprite[] WirelessSprites;
        public Sprite[] BatterySprites;
        public Sprite[] InputRateSprites;

        /// <summary>
        /// Updates wireless signal strength sprite 
        /// </summary>
        /// <param name="wirelessLevel"></param>
        public void UpdateWirelessSignalStrength(WirelessSignalStrengthLevel wirelessLevel)
        {
            WirelessImage.sprite = WirelessSprites[(int)wirelessLevel];
        }

        /// <summary>
        /// Updates battery level strength sprite 
        /// </summary>
        /// <param name="batteryLevel"></param>
        public void UpdateBatteryLevel(BatteryLevel batteryLevel)
        {
            BatteryImage.sprite = BatterySprites[(int)batteryLevel];
        }

        /// <summary>
        /// Updates input level strength sprite 
        /// </summary>
        /// <param name="inputLevel"></param>
        public void UpdateInputRate(InputRateLevel inputLevel)
        {
            InputRateImage.sprite = InputRateSprites[(int)inputLevel];
        }

        /// <summary>
        /// Validates image components and sprites. 
        /// </summary>
        private void OnValidate()
        {
            if (WirelessImage == null)
            {
                Debug.LogError("Missing WirelessImage components", this);
            }
            else
            {
                if (WirelessSprites == null || WirelessSprites.Length == 0)
                {
                    Debug.LogError("WirelessSprites contains no spries", this);
                }
                else
                {
                    WirelessImage.sprite = WirelessSprites[0];
                }
            }

            if (BatteryImage == null)
            {
                Debug.LogError("Missing WirelessImage components", this);
            }
            else
            {
                if (BatterySprites == null || BatterySprites.Length == 0)
                {
                    Debug.LogError("BatterySprites contains no spries", this);
                }
                else
                {
                    BatteryImage.sprite = BatterySprites[0];
                }
            }

            if (InputRateImage == null)
            {
                Debug.LogError("Missing WirelessImage components", this);
            }
            else
            {
                if (InputRateSprites == null || InputRateSprites.Length == 0)
                {
                    Debug.LogError("BatterySprites contains no spries", this);
                }
                else
                {
                    InputRateImage.sprite = InputRateSprites[0];
                }
            }
        }
    }
}
