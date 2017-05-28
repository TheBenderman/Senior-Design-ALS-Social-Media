using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.UITestComponents
{
    class DummySlider : Slider
    {
        public DummySlider()
        {
            minValue = 0;
            maxValue = 1;
            value = 1;
        }
    }
}
