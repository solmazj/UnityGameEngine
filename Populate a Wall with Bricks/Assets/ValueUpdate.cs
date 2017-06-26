using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ValueUpdate : MonoBehaviour 
{

	private Slider mySlider;
	private InputField myField;

	void Start() 
	{
		mySlider = gameObject.GetComponent<Slider>();
		myField = gameObject.GetComponent<InputField>();
	}

	public void UpdateValueFromFloat(float value) 
	{
		if (mySlider) { mySlider.value = value; }
		if (myField) { myField.text = value.ToString("n2"); }
	}

	public void UpdateValueFromString(string value) 
	{
		if (mySlider) { mySlider.value = float.Parse(value); }
		if (myField) { myField.text = value; }
	}


}