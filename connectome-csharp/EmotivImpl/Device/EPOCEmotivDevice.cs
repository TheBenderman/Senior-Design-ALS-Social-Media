using Emotiv;
using EmotivWrapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmotivWrapper;
using System.Diagnostics;
using EmotivWrapperInterface;

namespace EmotivImpl.Device
{
    public class EPOCEmotivDevice : EmotivDevice
    {
        #region private 
       private IntPtr eEvent;
       private IntPtr eState;

       private Stopwatch timer;

        private string username;
        private string password;
        private string profileName;
        #endregion

        public EPOCEmotivDevice()
        {
            this.username = string.Empty;
            this.password = string.Empty;
            this.profileName = string.Empty;
        }

        public EPOCEmotivDevice(string username, string password, string profileName)
        {
           this.username =  username;
           this.password =  password;
           this.profileName = profileName;
        }

        protected override bool ConnectionSetUp(out string errorMessage)
        {
            previousTimeElapsed = 0;

            //TODO get from constructor 
            string userName = string.IsNullOrEmpty(username) ? "kennanmeyer" : this.username; 
            string password = string.IsNullOrEmpty(this.password) ? "vd4kbPSAbYTt" : this.password;
            string profileName = string.IsNullOrEmpty(this.profileName) ? "NewProfile1" : this.profileName ;

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
                    errorMessage = ("Loading failed");
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

            errorMessage = string.Empty;

            return true; 
        }

        protected override bool DisconnectionSetUp()
        {
            EdkDll.IEE_EngineDisconnect();
            EdkDll.IEE_EmoStateFree(eState);
            EdkDll.IEE_EmoEngineEventFree(eEvent);

            return true; 
        }


        long previousTimeElapsed = 0;

        public override IEmotivState Read()
        {
            if (timer == null)
            { 
                timer = Stopwatch.StartNew();
            }

            long time = timer.ElapsedMilliseconds; 
            while (time == previousTimeElapsed)
            {
                time = timer.ElapsedMilliseconds; 
            }

            previousTimeElapsed = time; 

            int state = EdkDll.IEE_EngineGetNextEvent(eEvent);
            if (state != 0x600 && state != 0)
            {
                Debug.WriteLine(time +" "+ state);
            }

            if (state == EdkDll.EDK_OK)
            {
                var eventType = EdkDll.IEE_EmoEngineEventGetType(eEvent);
                //EdkDll.IEE_EmoEngineEventGetUserId(eEvent, engineUserID);
                //Debug.WriteLine(engineUserID);
                if (eventType == EdkDll.IEE_Event_t.IEE_EmoStateUpdated)
                {
                    int newState = EdkDll.IEE_EmoEngineEventGetEmoState(eEvent, eState);
                    var act = EdkDll.IS_GetWirelessSignalStatus(eState);
                    EdkDll.IEE_MentalCommandAction_t currentAction = EdkDll.IS_MentalCommandGetCurrentAction(eState);
                    float currentPower = EdkDll.IS_MentalCommandGetCurrentActionPower(eState);
                   
                    return new EmotivState() { power = currentPower, command = (EmotivStateType)currentAction, time = previousTimeElapsed };
                }
            }

            return new EmotivState() { power = 0, command = EmotivStateType.NULL, time = previousTimeElapsed };
            
        }
    }

}
