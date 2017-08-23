using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//runs on Update, aka updates every frame so not that efficient
public class ChangeLengthOfACube : MonoBehaviour {
	 
	public float xLength;
	public float yLength;
	public float zLength;

	void Update () {
		// changing the scale, thus length cause initial length is 1 units
		transform.localScale = new Vector3 (xLength, yLength, zLength); 

		// changing the y-component so that cube stays on the plane
		if (yLength != 1)
			transform.localPosition = new Vector3 (0, yLength/2, 0);
	}
}
