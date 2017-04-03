using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CoreTweet;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// Represents a ui component on the Twitter scene such as Authentication, timeline, messages etc
    /// </summary>
	public interface ITwitterComponent
    {
        void showComponent();
    }
}
