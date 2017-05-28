using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeObject : MonoBehaviour 
{
    [SerializeField] GameObject KeyboardPrefab;
	[SerializeField] GameObject secondScreenPrefab;


    void Start()
    {
        this.Inject();
    }

    public void OpenSecondScreen()
    {
        var s = Object.Instantiate(secondScreenPrefab);
        s.name = secondScreenPrefab.name;
        s.transform.SetParent(transform.parent, false);
    }
}
