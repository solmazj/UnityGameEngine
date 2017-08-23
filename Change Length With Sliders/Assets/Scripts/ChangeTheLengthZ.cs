using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//see ChangeTheLengthX script to read how this script works
public class ChangeTheLengthZ : MonoBehaviour {

	public GameObject cube;
	public Text text;
	public float sliderMaxValue;

	Slider mainSlider;


	void Awake () {
		mainSlider = this.gameObject.GetComponent<Slider> ();
	}


	void Update () {
		mainSlider.maxValue = sliderMaxValue;
		float zLength = mainSlider.value;
		text.text = mainSlider.value.ToString ("n2");
		cube.transform.localScale = new Vector3 (cube.transform.localScale.x, cube.transform.localScale.y, zLength);
	}
}
