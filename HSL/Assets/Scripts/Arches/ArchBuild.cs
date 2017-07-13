using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class ArchBuild : MonoBehaviour {

	public BezierCurve wireArc;
	public Transform brick;
	public int steps = 100; //public or private
	float[] arcLengths;
	float curveLength;
	float curveHeight;

	public void BuildAnArch () {
		//stop if any of the input is not supplied
		if (brick == null || wireArc == null) {
			Debug.Log ("Necessary input not provided");
			return;
		}

		ArcLengthsCalc ();
		//keeping track of brick number
		int brickNumber = 0;
		float brickLength = brick.GetComponent<WedgeBrickMesh> ().innerLength;
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		for (float u = (brickLength/2)/curveLength; u <= 1; u += brickLength/curveLength) {
			Transform item = Instantiate(brick) as Transform;
			Vector3 position = wireArc.GetPoint(Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt(position + wireArc.GetDirection(u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = transform;
			brickNumber++;
		}
		Debug.Log (brickNumber.ToString());
	}


	void ArcLengthsCalc () {
		//initialization
		arcLengths = new float[steps + 1];
		//initial information
		arcLengths [0] = 0;
		float ox = wireArc.GetPoint(0).x, oy = wireArc.GetPoint(0).y, clen = 0;
		//iterative calculation of points along the curve and the arc lengths
		for (int i = 1; i <= steps; i++) {
			float increment = 1f/steps, x = wireArc.GetPoint(i * increment).x, y = wireArc.GetPoint(i * increment).y;
			if (y > oy) {
				curveHeight = y;
			}
			float dx = ox - x, dy = oy - y;
			clen += Mathf.Sqrt (dx * dx + dy * dy);
			arcLengths [i] = clen;
			ox = x; oy=y;
		}
		//total curve length, also equal to arcLengths[len]
		Debug.Log(curveHeight.ToString());
		curveLength = clen;
	}


	float Map (float u) {
		float targetLength = u * curveLength; 
		//binary search of the point's index in the array that is biggest smaller length tha target length
		int low = 0, high = steps, index = 0;
		while (low < high) {
			index = low + ((high - low) / 2);
			if (arcLengths [index] < targetLength) {
				low = index + 1;
			} else {
				high = index;
			}
		}
		if (arcLengths [index] > targetLength) {
			index--;
		}
		float lengthBefore = arcLengths [index];
		if (lengthBefore == targetLength) {
			return index / steps;
		} else {
			return (index + (targetLength - lengthBefore) / (arcLengths [index + 1] - lengthBefore)) / steps;
		}
	}
}
