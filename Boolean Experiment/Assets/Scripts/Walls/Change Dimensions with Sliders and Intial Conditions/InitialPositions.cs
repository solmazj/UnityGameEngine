using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
//attach to any box whose dimensions need to be changed
//works with 3 sliders, x-y-z
public class InitialPositions : MonoBehaviour 
{

	public Slider[] sliders;
	GameObject wall;

	void Awake ()
	{
		wall = this.gameObject;
	}

	//always runs on update, even in edit mode. NOT THE BEST PRACTICE
	void Update () 
	{
		//If attached to a wall-to-be cube and plane as a ground at a point (x,0,z)
		//looks like the wall rises up and grows sideways from this point
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.localScale.y / 2, 
			this.transform.position.z);

		//there might be a more efficient way to do this
		foreach (Slider slider in sliders) 
		{
			if (slider.tag == "x") 
			{slider.value = wall.transform.localScale.x;} 
			else if (slider.tag == "y") 
			{slider.value = wall.transform.localScale.y;} 
			else 
			{slider.value = wall.transform.localScale.z;}

			//print the value of the sliders with 2 DP (decimal points)
			//if multiple text children exist in each slider, it uses hierarchy to decide which one to use
			slider.GetComponentInChildren<Text> ().text = slider.value.ToString("n2");
		}
	}
}
