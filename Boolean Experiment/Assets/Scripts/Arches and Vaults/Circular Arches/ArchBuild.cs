using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Diagnostics;


public class ArchBuild : MonoBehaviour {

	public int numberOfBricks;
	public float archThickness, brickDepth, vaultDepth;
	BezierCurve wireArc;
	int steps = 100;
	float[] arcLengths;
	float curveLength,  innerBrickLength, outerBrickLength;
	GameObject prefab, halfPrefab, halfDepthPrefab;
	CircularTunnel tunnel;


	void Awake () {
		tunnel = this.gameObject.AddComponent<CircularTunnel> ();
	}


	void PreliminaryCalc () { 
		//checking if the scene needs cleaning (if arches exist from before)
		GameObject[] arches = GameObject.FindGameObjectsWithTag ("Arch");
		for (int i = 0; i < arches.Length; i++) {
			Destroy (arches [i].gameObject);
		}

		//checking if the scene needs cleaning (if tunnels related to arches exist from before)
		Destroy(GameObject.Find("Tunnel"));

		//throwing exeptions if invalid input for vaults only, so if arch is built, this piece of code is not executed
		StackTrace stackTrace = new StackTrace();
		if (stackTrace.GetFrame(1).GetMethod().Name != "BuildAnArch") {
			if (vaultDepth <= brickDepth) {
				//throws an error message and pauses the game
				throw new Exception ("Vault depth cannot be less than brick depth. Check the input.");
				/*alternatively, if the error and halting of the program is unwanted, this piece of code could be used;
			it ignores the given vault depth and builds it with brick depth.
			if (vaultDepth <= brickDepth) {
//			if the vault depth should be used instead of brick depth, just execute the next line;
//			brickDepth = vaultDepth;
			BuildAnArch (); 
			return;*/
			}


			//checks if division is a whole number or not. Alternatively, if the error and halting of the program is
			//unwanted, something similar to changing the brick dimensions (could be manipulated through the wedge mesh)
			//as in DrawWall in WallBuild where full rows and a fraction of a row is drawn, could be done.
			float division = vaultDepth / brickDepth;
			if(!Mathf.Approximately(division-Mathf.Round(division),0)) {
				throw new Exception ("Vault depth has to be a multiple of brick depth. Check the input.");
			}
		}


		wireArc = this.gameObject.GetComponent<BezierCurve>();

		//check if less than 2 parameters are provided
		if ((!wireArc.fs && !wireArc.height) || (!wireArc.fs && !wireArc.embrasure) || (!wireArc.height && !wireArc.embrasure) || (!wireArc.height && !wireArc.embrasure && !wireArc.fs))
			throw new Exception ("Can not build an arch with less than two parameters. Check input.");

		if (!wireArc.same) {
			throw new Exception ("The three input parameters do not agree with each other. Check input or input only two parameters, the third one will be calculated automatically.");
		}

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
		tunnel.Tunnel ();
	}


	public void BuildARegularVault () {
		PreliminaryCalc ();
		for (int row = 1; row <= vaultDepth / brickDepth; row++) {
			OddRow ().transform.position = new Vector3 (0, 0, -vaultDepth / 2 + (row * brickDepth));
		}
		tunnel.Tunnel ();
	}


	public void BuildLongZipVault () {
		PreliminaryCalc ();
		int row = 1;
		//when calculating vaultDepth/brickDepth, it rounds it to int as in math
		while (row <= vaultDepth / brickDepth) {
			OddRow().transform.position = new Vector3 (0, 0, -vaultDepth/2 + (row * brickDepth));
			row++;
			if (row <= vaultDepth / brickDepth) {
				EvenRow().transform.position = new Vector3 (0, 0, -vaultDepth/2 + (row * brickDepth));
				row++;
			}
		}
		tunnel.Tunnel ();
	}


	public void BuildTravZipVault () {
		PreliminaryCalc ();
		FirstRow ().transform.position = new Vector3 (0, 0, vaultDepth/2);
		int j = 1;
		for (int i = 1; i < vaultDepth/brickDepth; i++) {
			IntermediateRow ().transform.position = new Vector3 (0, 0, vaultDepth / 2 - i * brickDepth);
			j = i + 1;
		}
		LastRow ().transform.position = new Vector3 (0, 0, vaultDepth/2 -  j * brickDepth + brickDepth / 2);
		tunnel.Tunnel ();
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
			item.transform.tag = "StructureBrick";
			item.gameObject.AddComponent<BoxCollider>();
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
				item.transform.tag = "StructureBrick";
				item.gameObject.AddComponent<BoxCollider>();
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
			item.transform.tag = "StructureBrick";
			item.gameObject.AddComponent<BoxCollider>();
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
				item.transform.tag = "StructureBrick";
				item.gameObject.AddComponent<BoxCollider>();
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
		float u = (3 * innerBrickLength/2)/curveLength;
		int i = 1;
		while (i < numberOfBricks) {
			Transform item = Instantiate (halfDepthPrefab.transform) as Transform;
			item.transform.tag = "StructureBrick";
			item.gameObject.AddComponent<BoxCollider>();
			Vector3 position = wireArc.GetPoint (Map (u));
			item.transform.localPosition = position;
			//align the bricks along the arc
			item.transform.LookAt (position + wireArc.GetDirection (u));
			//setting the object to which this script is attached as the parent of the created bricks
			item.transform.parent = parent.transform;
			u += 2 * innerBrickLength / curveLength;
			i += 2;
			//			if (i <= numberOfBricks) {
			//				item = Instantiate (prefab.transform) as Transform;
			//				position = wireArc.GetPoint (Map (u));
			//				position.z += brickDepth / 2;
			//				item.transform.localPosition = position;
			//				//align the bricks along the arc
			//				item.transform.LookAt (position + wireArc.GetDirection (u));
			//				//setting the object to which this script is attached as the parent of the created bricks
			//				item.transform.parent = parent.transform;
			//				u += innerBrickLength / curveLength;
			//				i++;
			//			}
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
			item.transform.tag = "StructureBrick";
			item.gameObject.AddComponent<BoxCollider>();
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
		item.transform.tag = "StructureBrick";
		item.gameObject.AddComponent<BoxCollider>();
		Vector3 position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = parent.transform;
		u = u + (3 * (innerBrickLength/curveLength) / 4);
		for (int i = 1; i < numberOfBricks; i++) {
			item = Instantiate (prefab.transform) as Transform;
			item.transform.tag = "StructureBrick";
			item.gameObject.AddComponent<BoxCollider>();
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
		item.transform.tag = "StructureBrick";
		item.gameObject.AddComponent<BoxCollider>();
		position = wireArc.GetPoint (Map (u));
		item.transform.localPosition = position;
		//align the bricks along the arc
		item.transform.LookAt (position + wireArc.GetDirection (u));
		//setting the object to which this script is attached as the parent of the created bricks
		item.transform.parent = parent.transform;
		return parent;
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

		//string Brick can be made into a parameter too, defines the texture

		if (prefab.GetComponent<MeshFilter> () == null) {
			prefab.AddComponent<MeshFilter> ();
		}
		if (prefab.GetComponent<MeshRenderer> () == null) {
			prefab.AddComponent<MeshRenderer> ();
		}

		prefab.GetComponent<MeshFilter>().mesh = CreateMesh(innerBrickLength, outerBrickLength, archThickness, brickDepth);
		prefab.GetComponent<MeshRenderer> ().material = Resources.Load ("Brick") as Material;

		AssetDatabase.CreateAsset (prefab.GetComponent<MeshFilter> ().sharedMesh, "Assets/Resources/WedgedBrick.asset");
	}


	void BuildHalfBrick () {
		//create empty halfPrefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		halfPrefab = PrefabUtility.CreatePrefab ("Assets/Resources/HalfWedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		//string Brick can be made into a parameter too, defines the texture

		if (halfPrefab.GetComponent<MeshFilter> () == null) {
			halfPrefab.AddComponent<MeshFilter> ();
		}
		if (halfPrefab.GetComponent<MeshRenderer> () == null) {
			halfPrefab.AddComponent<MeshRenderer> ();
		}




		halfPrefab.GetComponent<MeshFilter>().sharedMesh = CreateMesh (innerBrickLength/2, outerBrickLength/2, archThickness, brickDepth);
		halfPrefab.GetComponent<MeshRenderer> ().material = Resources.Load ("Brick") as Material;

		AssetDatabase.CreateAsset (halfPrefab.GetComponent<MeshFilter> ().sharedMesh, "Assets/Resources/HalfWedgedBrick.asset");
	}


	void BuildHalfDepthBrick () {
		//create empty halfDepthPrefab, and destroy the empty gameobject
		GameObject sampleBrick = new GameObject("SampleBrick");
		halfDepthPrefab = PrefabUtility.CreatePrefab ("Assets/Resources/HalfDepthWedgedBrick.prefab", sampleBrick);
		Destroy (sampleBrick); 
		//add the mesh component
		//string Brick can be made into a parameter too, defines the texture

		if (halfDepthPrefab.GetComponent<MeshFilter> () == null) {
			halfDepthPrefab.AddComponent<MeshFilter> ();
		}
		if (halfDepthPrefab.GetComponent<MeshRenderer> () == null) {
			halfDepthPrefab.AddComponent<MeshRenderer> ();
		}



		halfDepthPrefab.GetComponent<MeshFilter>().sharedMesh = CreateMesh (innerBrickLength, outerBrickLength, archThickness, brickDepth/2);
		halfDepthPrefab.GetComponent<MeshRenderer> ().material = Resources.Load ("Brick") as Material;

		AssetDatabase.CreateAsset (halfDepthPrefab.GetComponent<MeshFilter> ().sharedMesh, "Assets/Resources/HalfDepthWedgedBrick.asset");
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




	Mesh CreateMesh (float innerLength, float outerLength, float height, float depth) {
		//create the mesh
		Mesh mesh = new Mesh ();
		mesh.name = "WedgeBrick";

		//variables necessary for assigning vertices
		float inner = innerLength / 2;
		float outer = outerLength / 2;

		//assigning vertices
		Vector3[] vertices = new Vector3[24];
		vertices [0] = new Vector3 (0, 0, -inner);          //back left bottom
		vertices [1] = new Vector3 (0, 0, inner);			//back right bottom
		vertices [2] = new Vector3 (0, height, -outer);		//back left up
		vertices [3] = new Vector3 (0, height, outer);		//back right up
		vertices [4] = new Vector3 (depth, 0, -inner);		//front left bottom
		vertices [5] = new Vector3 (depth, 0, inner);		//front right bottom
		vertices [6] = new Vector3 (depth, height, -outer); //front left up
		vertices [7] = new Vector3 (depth, height, outer);	//front right up
		vertices [8] = new Vector3 (0, 0, inner);			//right face
		vertices [9] = new Vector3 (0, height, outer);
		vertices [10] = new Vector3 (depth, 0, inner);
		vertices [11] = new Vector3 (depth, height, outer);
		vertices [12] = new Vector3 (0, 0, -inner);			//left face
		vertices [13] = new Vector3 (0, height, -outer);
		vertices [14] = new Vector3 (depth, 0, -inner);
		vertices [15] = new Vector3 (depth, height, -outer);
		vertices [16] = new Vector3 (0, height, -outer);	//up face
		vertices [17] = new Vector3 (0, height, outer);
		vertices [18] = new Vector3 (depth, height, -outer);
		vertices [19] = new Vector3 (depth, height, outer);
		vertices [20] = new Vector3 (0, 0, -inner);			//bottom face
		vertices [21] = new Vector3 (0, 0, inner);
		vertices [22] = new Vector3 (depth, 0, -inner);
		vertices [23] = new Vector3 (depth, 0, inner);
		mesh.vertices = vertices;

		//assigning triangles
		int[] triangles = new int[] {
			3, 2, 0, 3, 0, 1, //front face
			5, 6, 7, 6, 5, 4,  //back face
			11, 9, 8, 11, 8, 10, //right face
			13, 15, 14, 13, 14, 12, //left face
			19, 18, 16, 19, 16, 17, //up face
			21, 20, 23, 20, 22, 23, //bottom face
		};
		mesh.triangles = triangles;

		//assigning uv for textures
		Vector2 uv0 = new Vector2 (0, 0);
		Vector2 uv1 = new Vector2 (1f, 0);
		Vector2 uv2 = new Vector2 (0, 1f);
		Vector2 uv3 = new Vector2 (1f, 1f);
		Vector2[] uvs = new Vector2 [] {
			uv0, uv1, uv2, uv3, //front face
			uv1, uv0, uv3, uv2, //back face
			uv0, uv2, uv1, uv3, //right face
			uv1, uv3, uv0, uv2, //left face
			uv0, uv1, uv2, uv3, //up face
			uv1, uv0, uv3, uv2, //bottom face
		};
		mesh.uv = uvs;

		//do lighting calcs
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();


		return mesh;
	}

}