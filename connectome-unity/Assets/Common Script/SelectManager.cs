using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour {
	public GameObject[] menuItems;
	public GameObject highlighter;
	private int selectedIndex = 0;

	public void next() 
	{
		selectedIndex = (selectedIndex + 1) % menuItems.Length;
		select(selectedIndex);
	}

	public void previous () 
	{
		selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
		select(selectedIndex);
	}

	private void select(int index) 
	{
		selectedIndex = index;
		highlighter.transform.SetParent(menuItems [index].transform);
		highlighter.transform.localPosition = new Vector2 (0, 0);
		highlighter.SetActive (true);
		Debug.Log ("The selected element = " + menuItems [index].name);
	}

}
