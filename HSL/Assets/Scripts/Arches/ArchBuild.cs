using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchBuild : MonoBehaviour {

	public BezierCurve wireArc;
	public Transform brick;
	public int len = 100; //public or private
	Vector3 a, b, c, d;
	float[] arcLengths;
	float curveLength;
	WedgeBrickMesh mesh;

	public void BuildAnArch () {
		//stop if any of the input is not supplied
		if (brick == null || wireArc == null) {
			Debug.Log ("Necessary input not provided");
			return;
		}

		ArcLengthsCalc ();
		mesh = brick.GetComponent<WedgeBrickMesh> ();
		float brickLength = mesh.outerLength + mesh.innerLength / 2;
		//keeping track of brick number
		int brickNumber = 0;
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		for (float u = (brickLength/2)/curveLength; u <= 1; u += brickLength/curveLength) {
			Transform item = Instantiate(brick) as Transform;
			Vector3 position = Bezier.GetPoint (a, b, c, d, Map (u));
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
		//initializations
		a = wireArc.points[0]; b = wireArc.points[1]; c = wireArc.points[2]; d = wireArc.points[3];
		arcLengths = new float[len + 1];
		//initial information
		arcLengths [0] = 0;
		float ox = Bezier.GetPoint(a,b,c,d,0).x, oy = Bezier.GetPoint(a,b,c,d,0).y, clen = 0;
		//iterative calculation of points along the curve and the arc lengths
		for (int i = 1; i <= len; i++) {
			float increment = 1f/len, x = Bezier.GetPoint(a,b,c,d,i * increment).x, y = Bezier.GetPoint(a,b,c,d,i * increment).y;
			float dx = ox - x, dy = oy - y;
			clen += Mathf.Sqrt (dx * dx + dy * dy);
			arcLengths [i] = clen;
			ox = x; oy=y;
		}
		//total curve length, also equal to arcLengths[len]
		curveLength = clen;
	}


	float Map (float u) {
		float targetLength = u * curveLength; 
		//binary search of the point's index in the array that is biggest smaller length tha target length
		int low = 0, high = len, index = 0;
		while (low < high) {
			index = low + ((high - low) / 2); //deleted the |0 part
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
			return index / len;
		} else {
			return (index + (targetLength - lengthBefore) / (arcLengths [index + 1] - lengthBefore)) / len;
		}
	}
}
