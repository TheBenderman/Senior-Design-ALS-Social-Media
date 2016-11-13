
using Emotiv;
using System;
using System.Diagnostics;
using System.Reflection;

public class EmotivSphero{

	public static IntPtr eEvent;
	public static IntPtr eState;
	public static int state = 0;
	public static uint engineUserID ;
	public static int userCloudID ;
	public static string profileName = null;
	public static int pending = 0;

    public static Action<string, float> Update = null;
    public static Action<string, float> Change = null;

    public static void Main(string[] args)
    {
        new EmotivSphero().Run(); 
    }


    public void Run()
    {

		string userName = "kennanmeyer";
		string password = "vd4kbPSAbYTt";
		string profileName = "NewProfile1";

		engineUserID = 0;
        userCloudID = -1; 

		if (EdkDll.IEE_EngineConnect("Emotiv Systems-5") != EdkDll.EDK_OK) {
			Debug.WriteLine("Emotiv Engine start up failed.");
			return;
		}

		eEvent = EdkDll.IEE_EmoEngineEventCreate();
		eState = EdkDll.IEE_EmoStateCreate();
		
		if (EmotivCloudClient.EC_Connect() != EmotivCloudClient.EC_OK) {
			Debug.WriteLine("Cannot connect to Emotiv Cloud");
			return;
		}

		if (EmotivCloudClient.EC_Login(userName, password) != EmotivCloudClient.EC_OK) {
			Debug.WriteLine("Your login attempt has failed. The username or password may be incorrect");
			return;
		}

		Debug.WriteLine("Logged in as " + userName);

		if (EmotivCloudClient.EC_GetUserDetail(ref userCloudID) != EmotivCloudClient.EC_OK) {
			return;
		}

		state = EdkDll.IEE_EngineGetNextEvent(eEvent);

		if (state == EdkDll.EDK_OK) {

			var eventType = EdkDll.IEE_EmoEngineEventGetType(eEvent);
			EdkDll.IEE_EmoEngineEventGetUserId(eEvent, out engineUserID);

			// Log the EmoState if it has been updated
			if (eventType == EdkDll.IEE_Event_t.IEE_UserAdded)
				Debug.WriteLine("User added");

		} else if (state != EdkDll.EDK_NO_EVENT) {
			Debug.WriteLine("Internal error in Emotiv Engine!");
			return;
		}

		Debug.WriteLine("userCloudID: "+userCloudID);
		Debug.WriteLine("userEngineID: "+engineUserID);

		int version	= -1;        // Lastest version
		int getNumberProfile = EmotivCloudClient.EC_GetAllProfileName(userCloudID);
    	Debug.WriteLine(getNumberProfile);

        if (getNumberProfile > 0){
        	int profileID = EmotivCloudClient.EC_GetProfileId(userCloudID, profileName);
        	Debug.WriteLine(profileID);
			if (EmotivCloudClient.EC_LoadUserProfile(userCloudID, (int)engineUserID, profileID, version) == EmotivCloudClient.EC_OK) {
				Debug.WriteLine("Loading finished"+EmotivCloudClient.EC_GetAllProfileName(userCloudID));
			} else {
            	Debug.WriteLine("Loading failed");
            	EmotivCloudClient.EC_Logout(userCloudID);
            	return;
            }

        }

        uint pTrainedActionsOut = 0; 
        EdkDll.IEE_MentalCommandGetTrainedSignatureActions(engineUserID, out pTrainedActionsOut);
		Debug.WriteLine("Current overall trained actions: " + pTrainedActionsOut);

        float skill = -1; 
		EdkDll.IEE_MentalCommandGetOverallSkillRating(engineUserID, out skill);
		Debug.WriteLine("Current overall skill rating: " + skill);
		
        //EdkDll.IEE_MentalCommandSetActionSensitivity(engineUserID, 1, 1,1,1);
        //EdkDll.IEE_MentalCommandSetSignatureCaching(engineUserID, 1);
        //EdkDll.IEE_MentalCommandSetActivationLevel(engineUserID, 1);
		
		startLiveClassificationProcess();
		
		Debug.WriteLine("Quitting...");

		EdkDll.IEE_EngineDisconnect();
		EdkDll.IEE_EmoStateFree(eState);
		EdkDll.IEE_EmoEngineEventFree(eEvent);
	}

	

	public static void startLiveClassificationProcess() {

		Debug.WriteLine("New State, Current Action, Current Power");

        EdkDll.IEE_MentalCommandAction_t? previousState = null; 


        while (true) {

			state = EdkDll.IEE_EngineGetNextEvent(eEvent);

            if (state == EdkDll.EDK_OK) {
				var eventType = EdkDll.IEE_EmoEngineEventGetType(eEvent);
				//EdkDll.IEE_EmoEngineEventGetUserId(eEvent, engineUserID);
				//Debug.WriteLine(engineUserID);
				if (eventType == EdkDll.IEE_Event_t.IEE_EmoStateUpdated) {
					int newState = EdkDll.IEE_EmoEngineEventGetEmoState(eEvent, eState);
                    var act  = EdkDll.IS_GetWirelessSignalStatus(eState);
					EdkDll.IEE_MentalCommandAction_t currentAction = EdkDll.IS_MentalCommandGetCurrentAction(eState);
					float currentPower = EdkDll.IS_MentalCommandGetCurrentActionPower(eState);

                    //Update(currentAction+"", currentPower);

                    if(previousState != currentAction)
                    {
                        Change(currentAction+"", currentPower);
                        previousState = currentAction;
                    }

                    Debug.WriteLine(act+" "+newState + ", " + currentAction + ", " + currentPower);
				}
			} 
		}
	}
}
