using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

//attaches to the sliders, facilitates connection between a slider and an input field attached to it
public class ValueUpdate : MonoBehaviour {

	Slider mySlider;
	InputField myField;


	void Start() {
		mySlider = gameObject.GetComponent<Slider>();
		myField = gameObject.GetComponent<InputField>();
	}


	//takes value from the slider and shows it in input field
	public void UpdateValueFromFloat(float value) {
		if (mySlider) { mySlider.value = value; }
		if (myField) { myField.text = value.ToString("n2"); }
	}


	//takes the value in input field and shows it in slider 
	public void UpdateValueFromString(string value) {
		if (String.IsNullOrEmpty (value)) {return; }
		if (mySlider) { mySlider.value = float.Parse(value); }
		if (myField) { myField.text = value; }
	}


}