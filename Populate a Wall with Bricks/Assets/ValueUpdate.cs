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
		Debug.Log("float value changed: " + value);
		if (mySlider) { mySlider.value = value; }
		if (myField) { myField.text = value.ToString(); }
	}

	public void UpdateValueFromString(string value) 
	{
		Debug.Log("string value changed: " + value);
		if (mySlider) { mySlider.value = float.Parse(value); }
		if (myField) { myField.text = value; }
	}


}