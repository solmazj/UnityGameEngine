using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using MeshMakerNamespace;

public class BrickWall : MonoBehaviour {


	GameObject prefab;
	public float brickLength, brickHeight, brickDepth, wallLength = 10f, wallHeight, wallDepth;
	float leftPoint = -5f;
	GameObject parent;


	void Awake () {
		parent = new GameObject("SolidWall");
	}


	public void BrickWallBuild () {

		CreatePrefabBrick();

		DrawWall (new Vector3 ((leftPoint - wallLength + brickLength / 2), brickHeight / 2, (-wallDepth + brickDepth) / 2));
	}

	void CreatePrefabBrick () {
		//creates a prefab with the given dimensions and color
		GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = new Vector3 (brickLength, brickHeight, brickDepth);
		PrefabUtility.CreatePrefab ("Assets/Resources/AbutmentBrick.prefab", brick);
		Destroy (brick);
		/*because it is a prefab, if the color of the modeled brick changes, the prefab color
		changes too even if this method is not called (in this case, even if button is not pressed*/
		prefab = Resources.Load ("AbutmentBrick") as GameObject;
		prefab.GetComponent<Renderer> ().material = Resources.Load ("Brick") as Material;
	}


	GameObject DrawBrick(Vector3 position) {
		//creates a brick from the prefab with its center in the given position
		GameObject brick = Instantiate (prefab) as GameObject;
		brick.transform.position = position;
		//tag name needs to be defined (added into tags) before runtime
		brick.tag = "Brick"; 
		brick.transform.parent = parent.transform;
		//if on last row, need to change the height, aka scale
		return brick;
	}


	void DrawHalfBrick(Vector3 position) {
		//creates a brick with half the length of the prefab with its center in the given position
		GameObject brick = DrawBrick (position);

		brick.transform.localScale = new Vector3 (brickLength / 2, prefab.transform.localScale.y, prefab.transform.localScale.z);
	}


	void DrawEndBrick (Vector3 position) {
		//creates a brick for the end of the row
		GameObject brick = DrawBrick (position);
		brick.transform.localScale = new Vector3 (Mathf.Abs(position.x - leftPoint) * 2, prefab.transform.localScale.y, prefab.transform.localScale.z);
	}


	void DrawOddRow (Vector3 position) {
		//position.x starts with negative numbers; populate as long as whole bricks fit
		while (Mathf.Abs(position.x) - Mathf.Abs(leftPoint) >= brickLength/2) 
		{
			DrawBrick (position);
			position.x += brickLength;
		}
		//draw end brick if one can is needed
		if (leftPoint + brickLength / 2 > position.x) 
		{
			position.x = leftPoint / 2 + position.x / 2 - brickLength / 4; 
			DrawEndBrick (position);
		}
	}

	void DrawEvenRow (Vector3 position) {
		//create half a brick first, and then repeat the process in DrawOddRow
		position.x = position.x - brickLength / 4;
		DrawHalfBrick (position);
		position.x += 3 * brickLength/ 4;
		while (Mathf.Abs(position.x) - Mathf.Abs(leftPoint) >= brickLength/2) {
			DrawBrick (position);
			position.x += brickLength;
		}
		//draw end brick if one can is needed
		if (leftPoint + brickLength / 2 > position.x) 
		{
			position.x = leftPoint / 2 + position.x / 2 - brickLength / 4; 
			DrawEndBrick (position);
		}
	}


	void DrawOddLane (Vector3 position) {
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				//draw a shorter brick if a whole brick can't fit and break because end of the lane
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				DrawOddRow (position);
				return;
			} else {
				DrawOddRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				//draw a shorter brick and break
				position.y = wallHeight/2 + position.y/2 - brickHeight/4; 
				DrawEvenRow (position);
				return;
			} else if ((position.y - brickHeight/2) < wallHeight){
				DrawEvenRow (position);
				position.y += brickHeight;
			}
		}
	}


	void DrawEvenLane (Vector3 position) {
		//same as DrawOddLane, but this starts with DrawEvenRow
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				DrawEvenRow (position);
				return;
			} else {
				DrawEvenRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				DrawOddRow (position);
				return;
			} else if ((position.y - brickHeight/2) < wallHeight) {
				DrawOddRow (position);
				position.y += brickHeight;
			}
		}
	}


	void DrawWall (Vector3 position) {
		while ((wallDepth - 2 * position.z) >= brickDepth)
		{
			DrawOddLane (position);
			position.z += brickDepth;
			if ((wallDepth-brickDepth) / 2 < position.z && position.z < (wallDepth + brickDepth) / 2) {
				//change the depth if whole brick depth does not fit and break
				position.z = wallDepth/4 + position.z/2 - brickDepth/4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z));
				DrawEvenLane (position);
				return;
			} else if ((wallDepth - 2 * position.z) >= brickDepth) {
				DrawEvenLane (position);
				position.z += brickDepth;
			}
			if ((wallDepth-brickDepth) / 2 < position.z && position.z < (wallDepth + brickDepth) / 2) {
				//change the depth if whole brick depth does not fit and break
				position.z = wallDepth / 4 + position.z / 2 - brickDepth / 4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z)); 
				DrawOddLane (position);
				return;
			}
		}
	}






	public void BrickDifference () {
		
		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG();

//		Debug.Log ("I am activated");

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
//		Debug.Log ("Number of bricks is " + bricks.Length);
		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere(brick.transform.position, brickLength);
//			Debug.Log ("Number of hitColliders is " + hitColliders.Length);
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders [i].gameObject.transform.tag == "StructureBrick") {

					csg.Brush = hitColliders[i].gameObject;
					csg.OperationType = CSG.Operation.Subtract;
					csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
					csg.useCustomMaterial = false; // Use the above material to fill cuts
					csg.hideGameObjects = false; // Hide target and brush objects after operation

					csg.keepSubmeshes = true; // Keep original submeshes and materials
					newOne = csg.PerformCSG();
					Destroy (csg.Target);
					csg.Target = newOne;
					newOne.name = "BrickWall";
					newOne.tag = "Brick";
//					Debug.Log ("I am in the if statement ivolving colliders");
				}
			}
		}
//		Destroy(GameObject.Find("Tunnel"));
	}



	public void Experimental () {
		
		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
		//		Debug.Log ("Number of bricks is " + bricks.Length);
		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere(brick.transform.position, brickLength);
			//			Debug.Log ("Number of hitColliders is " + hitColliders.Length);
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders [i].gameObject.transform.tag == "Tunnel") {

					csg.Brush = hitColliders[i].gameObject;
					csg.OperationType = CSG.Operation.Subtract;
					csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
					csg.useCustomMaterial = false; // Use the above material to fill cuts
					csg.hideGameObjects = false; // Hide target and brush objects after operation

					csg.keepSubmeshes = false; // Keep original submeshes and materials
					newOne = csg.PerformCSG();
					Destroy (csg.Target);
					csg.Target = newOne;
					newOne.name = "BrickWall";

					//					Debug.Log ("I am in the if statement ivolving colliders");
				}
			}
		}
				Destroy(GameObject.Find("Tunnel"));
	
	
	}
}