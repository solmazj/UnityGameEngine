using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//attach to a wall whose dimensions need to be changed
//works with 3 sliders, x-y-z
public class ChangeLengthSlider : MonoBehaviour {

	public Slider[] sliders;
	private GameObject wall;

	void Awake () {
		wall = this.gameObject;
	}


	void Update () {
		//change the dimensions of the wall
		wall.transform.localScale = new Vector3 (1.0f * sliders[0].value, 1.0f * sliders[1].value, 1.0f * sliders[2].value);
		//print the value of the sliders
		//if multiple text children exist in each slider, it uses hierarchy to see which one to use
		foreach (Slider slider in sliders) {
			Text value = slider.GetComponentInChildren<Text> ();
			value.text = slider.value.ToString ("n2");
		}
	}
}
