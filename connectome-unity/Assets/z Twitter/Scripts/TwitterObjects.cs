using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Connectome.Twitter.API;


public class TwitterObjects : MonoBehaviour
{
	public String[] objectsToManage = new String[]
	{
		"loginObjects",
		"homeTimeLineObjects",
		"homeObjects",
		"composeTweetObjects",
		"SelectConvObjects",
		"ViewConvObjects",
		"profileObjects"
	};
	public void setActiveObject(String objectName)
	{
		IEnumerable<UnityEngine.Object> all_Objs = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		all_Objs = all_Objs.Where(x => Array.FindIndex(objectsToManage, y => y.Equals(x.name)) > -1);

		foreach (UnityEngine.Object g in all_Objs)
		{
			GameObject gameobj = (GameObject)g;

			if (gameobj.name.Equals(objectName))
				gameobj.SetActive(true);
			else
				gameobj.SetActive(false);
		}
	}
}

