using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImageFlicker : MonoBehaviour
{
    public Color[] flicks;

    [Tooltip("Flicking interval in millisecond")]
    [Range(0, 1000)]
    public int interval = 0;

    private int flickIndex;
    private Image image; 

    void Start()
    {
        flickIndex = 0;
        image = GetComponent<Image>();

        StartCoroutine(flick());
    }


    void OnValidate()
    {
        if(flicks == null || flicks.Length == 0)
        {
            Debug.LogError("Flicker colors are null or empty", this);
        }

        if(GetComponent<Image>() == null)
        {
            Debug.LogError("Missing 'Image' component", this);
        }

    }

    private IEnumerator flick()
    {
        while (true)
        {
             yield return new WaitForSeconds((float) interval  / 1000);


            //flick 
            flickIndex = ++flickIndex % flicks.Length; 
            image.color = flicks[flickIndex]; 

        }


    }

}