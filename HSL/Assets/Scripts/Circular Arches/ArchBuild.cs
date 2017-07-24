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
	float curveLength,  innerBrickLength, outerBrickLength;
	GameObject prefab, halfPrefab;

	public void BuildAnArch () {
		//stop if the input is missing
		if (wireArc == null) {
			Debug.Log ("No arc provided");
			return;
		}

		//checking if the scene needs cleaning (if arch exists from before)
		GameObject arch = GameObject.FindGameObjectWithTag ("Arch");
		if (arch.transform.childCount > 0) {
			for (int i = 0; i < arch.transform.childCount; i++) {
				Destroy (arch.transform.GetChild (i).gameObject);
			}
		}

		//do arc calcs
		ArcLengthsCalc ();

		//the inner edge of the brick is along the arc
		innerBrickLength = curveLength / numberOfBricks;

		//make prefab bricks given the information about the arch thickness and the arc shape
		BuildFullBrick ();
		OddRaw ();
	}


	public void BuildARegularVault () {
		BuildAnArch ();
		//centering the vault about the origin in z direction
		GameObject.FindGameObjectWithTag("Arch").transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Arch").transform.position.x,
			GameObject.FindGameObjectWithTag("Arch").transform.position.y, -vaultDepth/2 + brickDepth);
		//copy created arch over however many times needed
		for (int rows = 1; rows < vaultDepth / brickDepth; rows++) {
			GameObject arch = Instantiate (GameObject.FindGameObjectWithTag("Arch")) as GameObject;
			arch.transform.position = new Vector3 (arch.transform.position.x, arch.transform.position.y,  arch.transform.position.z + rows * brickDepth);
		}
	}


	public void BuildLongZipVault () {
		BuildAnArch ();
		BuildHalfBrick ();
		//centering the vault about the origin in z direction
		GameObject.FindGameObjectWithTag("Arch").transform.position = new Vector3 (GameObject.FindGameObjectWithTag("Arch").transform.position.x,
			GameObject.FindGameObjectWithTag("Arch").transform.position.y, -vaultDepth/2 + brickDepth);
//		for (int rows = 1; rows < vaultDepth / brickDepth; rows++) {
			GameObject evenArch = Instantiate (GameObject.FindGameObjectWithTag("Arch")) as GameObject;
		for (int i = 0; i < evenArch.transform.childCount; i++) {
			Destroy (evenArch.transform.GetChild (i).gameObject);
		}
		EvenRaw ();
//		}
	}


	void OddRaw () {
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		for (int i = 1; i <= numberOfBricks; i++) {
			Transform item = Instantiate (prefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = transform;
			u += innerBrickLength / curveLength;
		}
	}


	void EvenRaw () {
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/4)/curveLength;
		Transform item = Instantiate (halfPrefab.transform) as Transform;
		Vector3 position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = transform;
		u = u + (3 * (innerBrickLength/curveLength) / 4);
		for (int i = 1; i < numberOfBricks; i++) {
			item = Instantiate (prefab.transform) as Transform;
			position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = transform;
			u += innerBrickLength / curveLength;
		}
		u = u - ((innerBrickLength / curveLength) / 4);
		item = Instantiate (halfPrefab.transform) as Transform;
		position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = transform;
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


	void BuildFullBrick () {
		//calculating the outer length of the brick
		float t1 = (innerBrickLength) / curveLength;
		float t2 = (2 * innerBrickLength) / curveLength;
		Vector3 tangent1 = wireArc.GetDirection (t1);
		Vector3 tangent2 = wireArc.GetDirection (t2);
		Vector3 scaled1 = new Vector3 (-archThickness * tangent1.y, archThickness *  tangent1.x, tangent1.z);
		Vector3 scaled2 = new Vector3 (-archThickness * tangent2.y, archThickness *  tangent2.x, tangent2.z);
		outerBrickLength = Vector3.Distance (wireArc.GetPoint (t1) + scaled1, wireArc.GetPoint (t2) + scaled2);

		//create empty prefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		prefab = PrefabUtility.CreatePrefab ("Assets/Resources/WedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		WedgeBrickMesh mesh = prefab.AddComponent<WedgeBrickMesh> ();
		//string Brick can be made into a parameter too, defines the texture
		mesh.CreateMesh (innerBrickLength, outerBrickLength, archThickness, brickDepth, "Brick");
	}


	void BuildHalfBrick () {
		//calculating the outer length of the brick
		float t1 = (innerBrickLength) / curveLength;
		float t2 = (2 * innerBrickLength) / curveLength;
		Vector3 tangent1 = wireArc.GetDirection (t1);
		Vector3 tangent2 = wireArc.GetDirection (t2);
		Vector3 scaled1 = new Vector3 (-archThickness * tangent1.y, archThickness *  tangent1.x, tangent1.z);
		Vector3 scaled2 = new Vector3 (-archThickness * tangent2.y, archThickness *  tangent2.x, tangent2.z);
		outerBrickLength = Vector3.Distance (wireArc.GetPoint (t1) + scaled1, wireArc.GetPoint (t2) + scaled2);

		//create empty prefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		halfPrefab = PrefabUtility.CreatePrefab ("Assets/Resources/HalfWedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		WedgeBrickMesh mesh = halfPrefab.AddComponent<WedgeBrickMesh> ();
		//string Brick can be made into a parameter too, defines the texture
		mesh.CreateMesh (innerBrickLength/2, outerBrickLength/2, archThickness, brickDepth, "Brick");
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
