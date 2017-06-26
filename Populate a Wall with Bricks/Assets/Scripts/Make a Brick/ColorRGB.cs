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
		Debug.Log ("I am running");
		red = Convert.ToByte(input);
		Debug.Log ("I got the input");
		brick.GetComponent<Renderer>().material.color = new Color32(red, green, blue, transparency);
		Debug.Log ("Should have changed the color by now");
	}

	public void ColorUpdateGreen (string input) 
	{
		green = Convert.ToByte(input);
		brick.GetComponent<Renderer>().material.color = new Color32(red, green, blue, transparency);
	}
	 
	public void ColorUpdateBlue (string input) 
	{
		blue = Convert.ToByte(input);
		brick.GetComponent<Renderer>().material.color = new Color32(red, green, blue, transparency);
	}
}
