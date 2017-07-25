using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class ArchBuild : MonoBehaviour {

	public int numberOfBricks;
	public float archThickness, brickDepth, vaultDepth;
	int steps = 100;
	float[] arcLengths;
	float curveLength,  innerBrickLength, outerBrickLength;
	GameObject prefab, halfPrefab, halfDepthPrefab;
	BezierCurve wireArc;

	void PreliminaryCalc () { 
		//checking if the scene needs cleaning (if arches exist from before)
		GameObject[] arches = GameObject.FindGameObjectsWithTag ("Arch");
		for (int i = 0; i < arches.Length; i++) {
			Destroy (arches [i].gameObject);
		}

		wireArc = this.gameObject.GetComponent<BezierCurve>();

		//do arc calcs
		ArcLengthsCalc ();

		//the inner edge of the brick is along the arc
		innerBrickLength = curveLength / numberOfBricks;

		//make prefab bricks given the information about the arch thickness and the arc shape
		BuildFullBrick ();
		BuildHalfBrick ();
		BuildHalfDepthBrick ();
	}


	public void BuildAnArch () {
		PreliminaryCalc ();
		//centering the created arch about the origin in z direction
		OddRow ().transform.position = new Vector3 (0, 0, brickDepth/2);
	}


	public void BuildARegularVault () {
		if (vaultDepth <= brickDepth) {
			BuildAnArch (); //ignores the given vault depth and builds it with brick depth, ask Becca if she wants it differently 
			return;
		}

		PreliminaryCalc ();
		for (int row = 1; row <= vaultDepth / brickDepth; row++) {
			OddRow ().transform.position = new Vector3 (0, 0, -vaultDepth / 2 + (row * brickDepth));
		}
	}


	public void BuildLongZipVault () {
		if (vaultDepth <= brickDepth) {
			BuildAnArch (); //ignores the given vault depth and builds it with brick depth, ask Becca if she wants it differently 
			return;
		}
			
		PreliminaryCalc ();
		int row = 0;
		//when calculating vaultDepth/brickDepth, it rounds it to int as in math
		while (row < vaultDepth / brickDepth) {
			row++;
			OddRow().transform.position = new Vector3 (0, 0, -vaultDepth/2 + (row * brickDepth));
			if (row <= vaultDepth / brickDepth) {
				row++;
				EvenRow().transform.position = new Vector3 (0, 0, -vaultDepth/2 + (row * brickDepth));
			}
		}
	}


	public void BuildTravZipVault () {
		if (vaultDepth <= brickDepth) {
			BuildAnArch (); //ignores the given vault depth and builds it with brick depth, ask Becca if she wants it differently 
			return;
		}

		PreliminaryCalc ();
		FirstRow ().transform.position = new Vector3 (0, 0, vaultDepth/2);
		for (int i = 1; i <= (vaultDepth/brickDepth - 1.5); i++) {
			IntermediateRow ().transform.position = new Vector3 (0, 0, vaultDepth / 2 - i * brickDepth);
		}
		LastRow ().transform.position = new Vector3 (0, 0, -vaultDepth/2 + brickDepth/2);
	}


	GameObject FirstRow () {
		//creating a parent  gameobject
		GameObject parent = new GameObject("FirstArch");
		parent.transform.tag = "Arch";
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		int i = 1;
		while (i <= numberOfBricks) {
			Transform item = Instantiate (prefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += innerBrickLength / curveLength;
			i++;
			if (i <= numberOfBricks) {
				item = Instantiate (halfDepthPrefab.transform) as Transform;
				position = wireArc.GetPoint (Map (u));
				item.transform.localPosition = position;
				//align the bricks along the arc
				item.transform.LookAt (position + wireArc.GetDirection (u));
				//setting the object to which this script is attached as the parent of the created bricks
				item.transform.parent = parent.transform;
				u += innerBrickLength / curveLength;
				i++;
			}
		}
		return parent;
	}


	GameObject IntermediateRow () {
		//creating a parent  gameobject
		GameObject parent = new GameObject("IntermediateArch");
		parent.transform.tag = "Arch";
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		int i = 1;
		while (i <= numberOfBricks) {
			Transform item = Instantiate (prefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += innerBrickLength / curveLength;
			i++;
			if (i <= numberOfBricks) {
				item = Instantiate (prefab.transform) as Transform;
				position = wireArc.GetPoint (Map (u));
				position.z += brickDepth / 2;
				item.transform.localPosition = position;
				//align the bricks along the arc
				item.transform.LookAt (position + wireArc.GetDirection (u));
				//setting the object to which this script is attached as the parent of the created bricks
				item.transform.parent = parent.transform;
				u += innerBrickLength / curveLength;
				i++;
			}
		}
		return parent;
	}


	GameObject LastRow () {
		//creating a parent  gameobject
		GameObject parent = new GameObject("LastArch");
		parent.transform.tag = "Arch";
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		int i = 1;
		while (i <= numberOfBricks) {
			Transform item = Instantiate (halfDepthPrefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += innerBrickLength / curveLength;
			i++;
			if (i <= numberOfBricks) {
				item = Instantiate (prefab.transform) as Transform;
				position = wireArc.GetPoint (Map (u));
				position.z += brickDepth / 2;
				item.transform.localPosition = position;
				//align the bricks along the arc
				item.transform.LookAt (position + wireArc.GetDirection (u));
				//setting the object to which this script is attached as the parent of the created bricks
				item.transform.parent = parent.transform;
				u += innerBrickLength / curveLength;
				i++;
			}
		}
		return parent;
	}


	GameObject OddRow () {
		//creating a parent  gameobject
		GameObject parent = new GameObject("OddArch");
		parent.transform.tag = "Arch";
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/2)/curveLength;
		for (int i = 1; i <= numberOfBricks; i++) {
			Transform item = Instantiate (prefab.transform) as Transform;
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += innerBrickLength / curveLength;
		}
		return parent;
	}


	GameObject EvenRow () {
		//creating a parent  gameobject
		GameObject parent = new GameObject("EvenArch");
		parent.transform.tag = "Arch";
		//parameter u for the arc length is like t for the curve; it's between 0 and 1
		float u = (innerBrickLength/4)/curveLength;
		Transform item = Instantiate (halfPrefab.transform) as Transform;
		Vector3 position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = parent.transform;
		u = u + (3 * (innerBrickLength/curveLength) / 4);
		for (int i = 1; i < numberOfBricks; i++) {
			item = Instantiate (prefab.transform) as Transform;
			position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += innerBrickLength / curveLength;
		}
		u = u - ((innerBrickLength / curveLength) / 4);
		item = Instantiate (halfPrefab.transform) as Transform;
		position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = parent.transform;
		return parent;
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
		//create empty halfPrefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		halfPrefab = PrefabUtility.CreatePrefab ("Assets/Resources/HalfWedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		WedgeBrickMesh mesh = halfPrefab.AddComponent<WedgeBrickMesh> ();
		//string Brick can be made into a parameter too, defines the texture
		mesh.CreateMesh (innerBrickLength/2, outerBrickLength/2, archThickness, brickDepth, "Brick");
	}


	void BuildHalfDepthBrick () {
		//create empty halfDepthPrefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		halfDepthPrefab = PrefabUtility.CreatePrefab ("Assets/Resources/HalfDepthWedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		WedgeBrickMesh mesh = halfDepthPrefab.AddComponent<WedgeBrickMesh> ();
		//string Brick can be made into a parameter too, defines the texture
		mesh.CreateMesh (innerBrickLength, outerBrickLength, archThickness, brickDepth/2, "Brick");
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