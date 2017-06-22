using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTheLengthY : MonoBehaviour {

	public Text text;
	public Slider mainSlider;
	public GameObject cube;
	public float sliderMaxValue;


	void Update () {
		mainSlider.maxValue = sliderMaxValue;
		float yLength = mainSlider.value;
		cube.transform.localScale = new Vector3 (cube.transform.localScale.x, yLength, cube.transform.localScale.z);
		if (yLength != 1) {
			cube.transform.position = new Vector3 (0, yLength / 2, 0);
		}
		text.text = mainSlider.value.ToString ();
	}
}
