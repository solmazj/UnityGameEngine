using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//see ChangeTheLengthX script to see how this script works, for one exception below
public class ChangeTheLengthY : MonoBehaviour {
	
	public GameObject cube;
	public Text text;
	public float sliderMaxValue;


	Slider mainSlider;


	void Awake () {
		mainSlider = this.gameObject.GetComponent<Slider> ();
	}

	void Update () {
		mainSlider.maxValue = sliderMaxValue;
		float yLength = mainSlider.value;
		text.text = mainSlider.value.ToString ("n2");
		cube.transform.localScale = new Vector3 (cube.transform.localScale.x, yLength, cube.transform.localScale.z);
		/* Unity positions objects according to their centroids, thus the initial cube with the scale of (1,1,1)
		 * is initially positioned at (0,0.5,0) to appear as if positioned on the "ground" plane which is positioned
		 * at (0,0,0). So when the height is changed, need to modify target object's position accordingly. If this
		 * part is not executed the bottom half of the object will appear below the "ground". */
		if (yLength != 1) {
			cube.transform.position = new Vector3 (0, yLength / 2, 0);
		}
	}
}
