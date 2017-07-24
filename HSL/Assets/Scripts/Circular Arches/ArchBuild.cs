using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class ArchBuild : MonoBehaviour {

	public BezierCurve wireArc;
	public int numberOfBricks = 25;
	public float archThickness, brickDepth, vaultDepth;
	int steps = 100;
	float[] arcLengths;
	float curveLength,  innerBrickLength;
	GameObject prefab;

	//how about tagging newly created bricks, and the arch itself
	public void BuildAVault () {
		BuildAnArch ();
		//copy created arch over a few times
		for (int rows = 1; rows < vaultDepth / brickDepth; rows++) {
			GameObject arch = Instantiate (GameObject.FindGameObjectWithTag("Arch")) as GameObject;
			arch.transform.position = new Vector3 (arch.transform.position.x, arch.transform.position.y,  arch.transform.position.z + rows * brickDepth);
		}
	}


	public void BuildAnArch () {
		//stop if the input is missing
		if (wireArc == null) {
			Debug.Log ("No arc provided");
			return;
		}

		//do arc calcs
		ArcLengthsCalc ();

		//the inner edge of the brick is along the arc
		innerBrickLength = curveLength / numberOfBricks;

		//make prefab bricks given the information about the arch thickness and the arc shape
		BuildTheBrick ();
	
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		for (int i = 1; i <= numberOfBricks; i++){
			Transform item = Instantiate(prefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint(Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt(position + wireArc.GetDirection(u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = transform;
			u += innerBrickLength/curveLength;
		}
	}

			
	void ArcLengthsCalc () {
		//initialization
		arcLengths = new float[steps + 1];
		//initial information
		float ox = wireArc.GetPoint(0).x, oy = wireArc.GetPoint(0).y, clen = 0;
		arcLengths [0] = clen;
		//iterative calculation of points along the curve and the arc lengths
		float increment = 1f/steps;
		for (int i = 1; i <= steps; i++) {
			float x = wireArc.GetPoint(i * increment).x, y = wireArc.GetPoint(i * increment).y;
			float dx = ox - x, dy = oy - y;
			clen += Mathf.Sqrt (dx * dx + dy * dy);
			arcLengths [i] = clen;
			ox = x; oy=y;
		}
		//total curve length, also equal to arcLengths[len]
		curveLength = clen;
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

		//create empty prefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		prefab = PrefabUtility.CreatePrefab ("Assets/Resources/WedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		WedgeBrickMesh mesh = prefab.AddComponent<WedgeBrickMesh> ();
		mesh.CreateMesh (innerBrickLength, outerBrickLength, archThickness, brickDepth, "Brick");
	}


	float Map (float u) {
		float targetLength = u * curveLength;
		//binary search of the point's index in the array that is biggest smaller length than target length
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
