﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//attach to any item (cube) whose dimensions need to be changed
//works with 3 sliders, x-y-z
public class ChangeLengthSlider : MonoBehaviour 
{

	public Slider[] sliders;
	private GameObject box;

	void Awake () 
	{
		box = this.gameObject;
	}
		
	public void PositionUpdate () 
	{
		//change the dimensions of the box, this change will be reflected in InitialPositions,
		//and values will appear in text box. Using Mathf.Round to display 2 DP in the inspector
		box.transform.localScale = new Vector3 (Mathf.Round(sliders[0].value * 100)/100, 
			Mathf.Round(sliders[1].value * 100)/100, Mathf.Round(sliders[2].value * 100)/100);
	}
}
