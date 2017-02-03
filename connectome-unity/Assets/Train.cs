using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour {

    public Button button;
    public Slider slider;
    private string[] testValues;
    private const string yes = "Yes";
    private const string no = "No";


    // Use this for initialization
    void Start () {
        createTestArray();
        Acrivate();

    }

    void createTestArray()
    {
        testValues = new string[] { yes, yes, yes, no, no, no };
        System.Random rnd = new System.Random();
        testValues = testValues.OrderBy(x => rnd.Next()).ToArray();
    }	
	// Update is called once per frame
	void Update () {
        slider.value += Time.deltaTime;
		
	}

    void Acrivate()
    {
        StartCoroutine(Phases());
    }

    IEnumerator Phases()
    {
        int pos = 0;
        Debug.Log(testValues.ToString());
        while (true)
        {
            yield return new WaitForSeconds(3);
            button.transform.GetChild(0).GetComponent<Text>().text = Convert.ToString(testValues[pos]);
            slider.value = slider.minValue;
            if (pos < testValues.Length)
            {
                pos++;
            }  
        }

    }

}
