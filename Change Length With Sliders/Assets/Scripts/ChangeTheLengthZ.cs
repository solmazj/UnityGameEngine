using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTheLengthZ : MonoBehaviour {

	public Text text;
	public Slider mainSlider;
	public GameObject cube;
	public float sliderMaxValue;


	void Update () {
		mainSlider.maxValue = sliderMaxValue;
		float zLength = mainSlider.value;
		cube.transform.localScale = new Vector3 (cube.transform.localScale.x, cube.transform.localScale.y, zLength);
		text.text = mainSlider.value.ToString ();
	}
}
