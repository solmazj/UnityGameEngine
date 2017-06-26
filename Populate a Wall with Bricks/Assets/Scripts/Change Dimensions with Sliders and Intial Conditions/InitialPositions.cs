using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
//attach to any cube whose dimensions are changed
//works with 3 sliders, x-y-z
public class InitialPositions : MonoBehaviour 
{

	public Slider[] sliders;
	private GameObject wall;

	void Awake ()
	{
		wall = this.gameObject;
	}

	void Update () 
	{
		//If attached to a wall-to-be cube and plane as a ground at (x,0,z)
		//Looks like the wall rises up from the starting point, and grows sideways from this point
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.localScale.y / 2, this.transform.position.z);

		//there might be a more efficient way to do this
		foreach (Slider slider in sliders) 
		{
			if (slider.tag == "x") 
			{slider.value = wall.transform.localScale.x;} 
			else if (slider.tag == "y") 
			{slider.value = wall.transform.localScale.y;} 
			else 
			{slider.value = wall.transform.localScale.z;}

			//print the value of the sliders
			//if multiple text children exist in each slider, it uses hierarchy to see which one to use
			slider.GetComponentInChildren<Text> ().text = slider.value.ToString("n2");
		}
	}
}
