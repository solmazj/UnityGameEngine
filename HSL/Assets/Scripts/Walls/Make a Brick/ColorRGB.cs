using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//attach to the brick to change its color (instead, textures can be used as well, which is much easier)
public class ColorRGB : MonoBehaviour {

	//transparency of a stone is set for 255, meaning it's opaque, but other values do not seem to work here anyway
	byte transparency = 255;
	//RGB values are set to be 255 as a default color
	byte red = 255, green = 255, blue = 255;


	//next 3 function are called from input fields to set RGB colors
	public void ColorUpdateRed (string input) {
		red = BoundaryConditions (input);
		ColorUpdate ();
	}


	public void ColorUpdateGreen (string input) {
		green = BoundaryConditions (input);
		ColorUpdate ();
	}

	 
	public void ColorUpdateBlue (string input) {
		blue = BoundaryConditions (input);
		ColorUpdate ();
	}


	byte BoundaryConditions (string input) {
		//if the field is empty, go back to default number of 255
		if (String.IsNullOrEmpty(input)) {return (byte)255;}
		//if not empty, clamp it to 0-255, so even if it the input is out the range, it will be made a valid input
		int integer = Mathf.Clamp (int.Parse(input), 0, 255);
		return (byte)integer;
	} 


	void ColorUpdate () {
		this.gameObject.GetComponent<Renderer>().material.color = new Color32(red, green, blue, transparency);
	}
}



