using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//attach to the brick to change its color
public class ColorRGB : MonoBehaviour 
{

	private GameObject brick;
	//transparency of a stone is usually 255, but in case the script is used for other materials
	//another method with transparency can be written, although does not seem to work for now
	private byte transparency = 255;
	private byte red = 255;
	private byte green = 255;
	private byte blue = 255;

	void Awake () 
	{
		brick = this.gameObject;
	}

	public void ColorUpdateRed (string input) 
	{
		red = BoundaryConditions (input);
		ColorUpdate ();
	}

	public void ColorUpdateGreen (string input) 
	{
		green = BoundaryConditions (input);
		ColorUpdate ();
	}
	 
	public void ColorUpdateBlue (string input) 
	{
		blue = BoundaryConditions (input);
		ColorUpdate ();
	}

	byte BoundaryConditions (string input) 
	{
		if (String.IsNullOrEmpty (input)) {return (byte)255;}

		int integer = Mathf.Clamp (int.Parse (input), 0, 255);
		return (byte)integer;
	} 

	void ColorUpdate ()
	{
		brick.GetComponent<Renderer>().material.color = new Color32(red, green, blue, transparency);
	}
}



