using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

//attaches to the wire arc
/*Row is rows seen from the front (x-y plane), Lane is rows seen from the top (x-z plane)
the constant numbers correspond to the cases when InitialPositions script is enabled*/
public class Abutments : MonoBehaviour {


	GameObject prefab;
	public float brickLength, brickHeight, brickDepth, wallLength, wallHeight, wallDepth;
	float topHeight;
	float leftPoint;
	GameObject parent;


	public void SetAbutmentPositions(float pos) {
		leftPoint = pos;
	}

	public void BuildAbutment () {
		//cleaning the objects created before
		GameObject[] abutments = GameObject.FindGameObjectsWithTag ("Abutment");
		for (int i = 0; i < abutments.Length; i++) {
			Destroy (abutments [i].gameObject);
		}

		//creating a parent object. Position will be (0,0,0)
		parent = new GameObject("Abutment");
		parent.transform.tag = "Abutment";
			
		CreatePrefabBrick();

		if (wallDepth <= brickDepth) {
			throw new Exception ("Abutment depth cannot be less than the abutment brick depth.");
		} else {
			//create the left abutment
			DrawWall (new Vector3 ((leftPoint - wallLength + brickLength / 2), brickHeight / 2, (-wallDepth + brickDepth) / 2));

			//copy the left abutment over to the right side
			GameObject rightAbutment = Instantiate (parent) as GameObject;
			rightAbutment.transform.position = new Vector3 (wallLength + Mathf.Abs(leftPoint) * 2, 
				parent.transform.position.y, parent.transform.position.z);
		}
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
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (brick.transform.localScale.x, 
				topHeight, brick.transform.localScale.z);
		}
		return brick;
	}
		

	void DrawHalfBrick(Vector3 position) {
		//creates a brick with half the length of the prefab with its center in the given position
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (brickLength / 2, topHeight, prefab.transform.localScale.z);
		} else {
			brick.transform.localScale = new Vector3 (brickLength / 2, prefab.transform.localScale.y, prefab.transform.localScale.z);
		}
	}


	void DrawEndBrick (Vector3 position) {
		//creates a brick for the end of the row
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (Mathf.Abs(position.x - leftPoint) * 2, topHeight, prefab.transform.localScale.z);
		}
		else {
			brick.transform.localScale = new Vector3 (Mathf.Abs(position.x - leftPoint) * 2, prefab.transform.localScale.y, prefab.transform.localScale.z);
		}
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
		topHeight = brickHeight;
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				//draw a shorter brick if a whole brick can't fit and break because end of the lane
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				topHeight = 2 * (wallHeight-position.y);
				DrawOddRow (position);
				return;
			} else {
				DrawOddRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				//draw a shorter brick and break
				position.y = wallHeight/2 + position.y/2 - brickHeight/4; 
				topHeight = 2 * (wallHeight-position.y);
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
		topHeight = brickHeight;
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				topHeight = 2 * (wallHeight-position.y);
				DrawEvenRow (position);
				return;
			} else {
				DrawEvenRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				topHeight = 2 * (wallHeight-position.y);
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
}