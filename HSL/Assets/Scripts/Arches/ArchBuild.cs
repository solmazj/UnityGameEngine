using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class ArchBuild : MonoBehaviour {

	public BezierCurve wireArc;
//	public Transform brick;
	public int numberOfBricks = 25;
	public int steps = 100;
	public float brickDepth, archDepth, archThickness;
	float[] arcLengths;
	float curveLength, archHeight,  innerBrickLength;

//	public void BuildAnArch () {
//		//stop if the input is missing
//		if (wireArc == null) {
//			Debug.Log ("No arc provided");
//			return;
//		}
//
//		//do arc calcs
//		ArcLengthsCalc ();
//
//		//the inner edge of the brick is along the arc
//		innerBrickLength = curveLength / numberOfBricks;
//		//parameter u for the arc length is like t for the curve; it's between 0 and 1
//		float u = (brickLength/2)/curveLength;
//		for (int i = 1; i <= numberOfBricks; i++){
//			Transform item = Instantiate(brick) as Transform;
//			Vector3 position = wireArc.GetPoint(Map (u));
//			item.transform.localPosition = position;
//			//align the bricks along the arc
//			item.transform.LookAt(position + wireArc.GetDirection(u));
//			//setting the object to which this script is attached as the parent of the created bricks
//			item.transform.parent = transform;
//			u += brickLength/curveLength;
//		}
//		//remove the brick from the scene
////		brick.gameObject.SetActive (false);
//
//	}

	public void Experiment() {
		ArcLengthsCalc ();
		innerBrickLength = curveLength / numberOfBricks;
		BuildTheBrick ();
	}

	void BuildTheBrick () {
		//calculating the outer length of the brick
		float t1 = (innerBrickLength) / curveLength;
		float t2 = (2 * innerBrickLength) / curveLength;
		Vector3 tangent1 = wireArc.GetDirection (t1);
		Vector3 tangent2 = wireArc.GetDirection (t2);
		Vector3 scaled1 = new Vector3 (-archThickness * tangent1.y, archThickness *  tangent1.x, tangent1.z);
		Vector3 scaled2 = new Vector3 (-archThickness * tangent2.y, archThickness *  tangent2.x, tangent2.z);
		float outerBrickLength = Vector3.Distance (wireArc.GetPoint (t1) + scaled1, wireArc.GetPoint (t2) + scaled2);
		Debug.Log (innerBrickLength + " + " + outerBrickLength);
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
				archHeight = y;
			}
			float dx = ox - x, dy = oy - y;
			clen += Mathf.Sqrt (dx * dx + dy * dy);
			arcLengths [i] = clen;
			ox = x; oy=y;
		}
		//total curve length, also equal to arcLengths[len]
		curveLength = clen;
		//Height and free span of the arch
		Debug.Log("The height of the arch is " + archHeight.ToString());
		Debug.Log("The free span of the arch is " + wireArc.FreeSpan().ToString());
	}


	float Map (float u) {
		float targetLength = u * curveLength; 
		//binary search of the point's index in the array that is biggest smaller length tha target length
		int low = 0, high = numberOfBricks, index = 0;
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
			return index / numberOfBricks;
		} else {
			return (index + (targetLength - lengthBefore) / (arcLengths [index + 1] - lengthBefore)) / numberOfBricks;
		}
	}
}
