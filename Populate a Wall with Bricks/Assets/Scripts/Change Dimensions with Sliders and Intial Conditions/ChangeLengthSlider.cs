using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attach to any cube whose dimensions need to be changed
//works with 3 sliders, x-y-z
public class ChangeLengthSlider : MonoBehaviour 
{

	public Slider[] sliders;
	private GameObject wall;

	void Awake () 
	{
		wall = this.gameObject;
	}
		
	public void PositionUpdate () 
	{
		//change the dimensions of the wall, this update will call InitialSliderValue, and value will appear in text box
		wall.transform.localScale = new Vector3 (1.0f * sliders[0].value, 1.0f * sliders[1].value, 1.0f * sliders[2].value);
	}
}
