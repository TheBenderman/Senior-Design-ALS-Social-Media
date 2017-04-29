using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class RefreshClickSlider : MonoBehaviour
{
    public Slider Refresh;
    public Slider Click; 

    public void SetClickValue(float f)
    {
        Click.value = f; 
    }

    public void SetRefreshValue(float f)
    {
        Refresh.value = f; 
    }
	 
}