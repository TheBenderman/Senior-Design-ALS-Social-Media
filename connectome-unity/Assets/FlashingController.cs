using System.Collections;
using Connectome.Unity.UI;

using UnityEngine.UI;
using UnityEngine;

public class FlashingController : MonoBehaviour {

    public ImageFlicker high;
    public Text text; 

	public void UpdateValue(Slider s)
    {
        high.Frequency = (int) s.value;
        text.text = "" + ((int)s.value);
    }
}
