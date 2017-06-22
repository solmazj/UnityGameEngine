using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

	public class ChangeTheLengthX : MonoBehaviour {

	public Text text;
	public Slider mainSlider;
	public GameObject cube;
	public float sliderMaxValue;


	void Update () {
		mainSlider.maxValue = sliderMaxValue;
		float xLength = mainSlider.value;
		cube.transform.localScale = new Vector3 (xLength, cube.transform.localScale.y, cube.transform.localScale.z);
		text.text = mainSlider.value.ToString ();
	}
}
