using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
//attach to the cube
//works with 3 sliders, x-y-z
public class InitialSliderValue : MonoBehaviour {

	public Slider[] sliders;
	private GameObject wall;

	void Update () {
		wall = this.gameObject;
		//there might be a more efficient way to do this
		foreach (Slider slider in sliders) {
			if (slider.tag == "x") {
				slider.value = wall.transform.localScale.x;
			} else if (slider.tag == "y") {
				slider.value = wall.transform.localScale.y;
			} else {
				slider.value = wall.transform.localScale.z;
			}
			Text value = slider.GetComponentInChildren<Text> ();
			value.text = slider.value.ToString("n2");
		}
	}
}
