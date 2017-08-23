using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//is attached to the corresponding slider
//works on Update, aka updates each frame. Not very efficient but good for starters and understanding how Unity works
	public class ChangeTheLengthX : MonoBehaviour {

	//the object whose dimensions are being changed with the slider
	public GameObject cube;
	//text field where the value of the slider is going to be displayed
	public Text text;
	//the max value of the dimension (totally optional)
	public float sliderMaxValue;

	//the slider that controls length parameter
	Slider mainSlider;


	//Awake is called once right after the game is started, used for initialization
	void Awake () {
		/* Gets the slider component of the gameobject that this script is attached to, aka Slider.
		 * But because slider is not a gameObject but a UI script that is attached to an (empty?) gameObject,
		 * need to first access the gameObject itself, then the characteristic script to access the slider behavior. */  
		mainSlider = this.gameObject.GetComponent<Slider> ();
	}


	void Update () {
		//setting the max value of the dimension
		mainSlider.maxValue = sliderMaxValue;
		//obtain the value of the slider
		float xLength = mainSlider.value;
		//display the value with 2 decimal points ("n2") in the text field
		text.text = mainSlider.value.ToString ("n2");
		//manipulate the dimension of the object, preserving the other dimensions as they were before
		cube.transform.localScale = new Vector3 (xLength, cube.transform.localScale.y, cube.transform.localScale.z);
	}
}
