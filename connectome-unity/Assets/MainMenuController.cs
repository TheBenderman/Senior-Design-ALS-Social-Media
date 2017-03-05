using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main controller in main menu 
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Luanches Twittet scene. 
    /// </summary>
	public void GoToTweet()
    {
        SceneManager.LoadScene("Twitter"); 
    }
}
