using Connectome.Emotiv.Common;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Template;
using Emotiv;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connectome.Emotiv.Implementation
{
    /// <summary>
    /// Connects and Reads states from a physical EPOC Emotiv device 
    /// </summary>
    public class EPOCEmotivDevice : EmotivDevice
    {
        #region Private Attributes 
        /// <summary>
        /// Holds buffered emotiv event id 
        /// </summary>
        private IntPtr eEvent;

        /// <summary>
        /// Holds emotiv device connection state 
        /// </summary>
        private IntPtr eState;

        /// <summary>
        /// Holds emotiv account username 
        /// </summary>
        private string username;
        
        /// <summary>
        /// Holds emotiv account password 
        /// </summary>
        private string password;

        /// <summary>
        /// Holds emotiv account profile which contains a trained profile. 
        /// </summary>
        private string profileName;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a device with the appropriate login credentials.   
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="profileName"></param>
        public EPOCEmotivDevice(string username, string password, string profileName)
        {
            this.username = username;
            this.password = password;
            this.profileName = profileName;
        }
        #endregion
        #region Overrides
        protected override bool ConnectionSetUp(out string errorMessage)
        {
            string userName = this.username;
            string password = this.password;
            string profileName = this.profileName;

            int state = 0;

            uint engineUserID;
            int userCloudID;
            engineUserID = 0;
            userCloudID = -1;

            if (EdkDll.IEE_EngineConnect("Emotiv Systems-5") != EdkDll.EDK_OK)
            {
                errorMessage = "Emotiv Engine start up failed.";
                return false;
            }

            eEvent = EdkDll.IEE_EmoEngineEventCreate();
            eState = EdkDll.IEE_EmoStateCreate();

            UpdateBatteryLevel();
            UpdateWirelessSigmalStrength();

            if (EmotivCloudClient.EC_Connect() != EmotivCloudClient.EC_OK)
            {
                errorMessage = ("Cannot connect to Emotiv Cloud");
                return false;
            }

            if (EmotivCloudClient.EC_Login(userName, password) != EmotivCloudClient.EC_OK)
            {
                errorMessage = ("Your login attempt has failed. The username or password may be incorrect");
                return false;
            }

            //Debug.WriteLine("Logged in as " + userName);

            if (EmotivCloudClient.EC_GetUserDetail(ref userCloudID) != EmotivCloudClient.EC_OK)
            {
                errorMessage = "Unable to get user details";
                return false;
            }

            state = EdkDll.IEE_EngineGetNextEvent(eEvent);

            if (state == EdkDll.EDK_OK)
            {

                var eventType = EdkDll.IEE_EmoEngineEventGetType(eEvent);
                EdkDll.IEE_EmoEngineEventGetUserId(eEvent, out engineUserID);

                // Log the EmoState if it has been updated
                if (eventType == EdkDll.IEE_Event_t.IEE_UserAdded)
                {
                    //Debug.WriteLine("User added");
                }

            }
            else if (state != EdkDll.EDK_NO_EVENT)
            {
                errorMessage = ("Internal error in Emotiv Engine!");
                return false;
            }

            //Debug.WriteLine("userCloudID: " + userCloudID);
            //Debug.WriteLine("userEngineID: " + engineUserID);

            int version = -1;        // Lastest version
            int getNumberProfile = EmotivCloudClient.EC_GetAllProfileName(userCloudID);
            //Debug.WriteLine(getNumberProfile);

            if (getNumberProfile > 0)
            {
                int profileID = EmotivCloudClient.EC_GetProfileId(userCloudID, profileName);
                //Debug.WriteLine(profileID);
                if (EmotivCloudClient.EC_LoadUserProfile(userCloudID, (int)engineUserID, profileID, version) == EmotivCloudClient.EC_OK)
                {
                    // Debug.WriteLine("Loading finished" + EmotivCloudClient.EC_GetAllProfileName(userCloudID));
                }
                else
                {
                    errorMessage = ("Loading failed: either USB not connected or profile  \'" + profileName + "\' doesn't exist");
                    EmotivCloudClient.EC_Logout(userCloudID);
                    return false;
                }

            }

            uint pTrainedActionsOut = 0;
            EdkDll.IEE_MentalCommandGetTrainedSignatureActions(engineUserID, out pTrainedActionsOut);
            //Debug.WriteLine("Current overall trained actions: " + pTrainedActionsOut);

            float skill = -1;
            EdkDll.IEE_MentalCommandGetOverallSkillRating(engineUserID, out skill);
            //Debug.WriteLine("Current overall skill rating: " + skill);

            errorMessage = "Profile " + profileName + " was loaded and device connected!";
            IsConnected = true; 
            return true;
        }

        protected override bool DisconnectionSetUp(out string msg)
        {
            try
            {
                if (IsConnected)
                {
                    Thread disconnectThread = new Thread(() => {
                        EdkDll.IEE_EngineDisconnect();
                        EdkDll.IEE_EmoStateFree(eState);
                        EdkDll.IEE_EmoEngineEventFree(eEvent);
                    });

                    disconnectThread.Start();

                    disconnectThread.Join(1000); 
                }
                else
                {
                    msg = "Device is not connected";
                    IsConnected = false; 
                    return true;
                }
            }
            catch (Exception e)
            {
                msg = e.ToString();
                return false;
            }

            msg = "success";
            return true;
        }

        public int[] ContactQuality; 

        public override IEmotivState AttemptRead(long time)
        {
            UpdateBatteryLevel();
            UpdateWirelessSigmalStrength();

            //EdkDll.IEE_EEG_ContactQuality_t[] Quality;

            //EdkDll.IS_GetContactQualityFromAllChannels(this.eState, out Quality);

            //ContactQuality = new int[Quality.Length];

            /*for (int i = 0; i < Quality.Length; i++)
            {
                //ContactQuality[i] = (int)Quality[i];

                //Debug.Write(Quality[i]+ ", ");
            }*/

            //Debug.WriteLine("");

            int state = EdkDll.IEE_EngineGetNextEvent(eEvent);

            if (state == EdkDll.EDK_OK)
            {
                var eventType = EdkDll.IEE_EmoEngineEventGetType(eEvent);
                //EdkDll.IEE_EmoEngineEventGetUserId(eEvent, engineUserID);
                //Debug.WriteLine(engineUserID);
                if (eventType == EdkDll.IEE_Event_t.IEE_EmoStateUpdated)
                {
                   /* for (int i = 0; i < Quality.Length; i++)
                    {
                        //ContactQuality[i] = (int)Quality[i];

                        Debug.Write((int)Quality[i] + ", ");
                    }

                    Debug.WriteLine("");*/


                    int newState = EdkDll.IEE_EmoEngineEventGetEmoState(eEvent, eState);
                    var act = EdkDll.IS_GetWirelessSignalStatus(eState);
                    EdkDll.IEE_MentalCommandAction_t currentAction = EdkDll.IS_MentalCommandGetCurrentAction(eState);
                    float currentPower = EdkDll.IS_MentalCommandGetCurrentActionPower(eState);

                    return new EmotivState((EmotivCommandType)currentAction, currentPower, time);
                }
            }

            return new EmotivState(EmotivCommandType.NULL, 0f, time);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Update battery level variables
        /// </summary>
        private void UpdateBatteryLevel()
        {
            int maxLevel, batteryLevel;
            EdkDll.IS_GetBatteryChargeLevel(this.eState, out batteryLevel, out maxLevel);

            this.BatteryLevel = batteryLevel;
        }

        /// <summary>
        /// Update wireless signal strength variable 
        /// </summary>
        private void UpdateWirelessSigmalStrength()
        {
            EdkDll.IEE_SignalStrength_t s = EdkDll.IS_GetWirelessSignalStatus(this.eState);

            this.WirelessSignalStrength = (int)s; 
        }
        #endregion
    }
}
