using Connectome.Emotiv.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.UITestComponents
{
    class DummyDropdown : Dropdown
    {
        public DummyDropdown()
        {
            value = (int)EmotivCommandType.PUSH;
            AddOptions(new List<string>(new string[] { "None", "Push", "Pull" }));
        }
    }
}
