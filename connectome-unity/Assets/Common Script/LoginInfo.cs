using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginInfo : MonoBehaviour {
    private string UserName = "";
    private string UserProfile = "";

    public string Login { get { return UserName; } }
    public string Profile { get { return UserProfile; } }
}
