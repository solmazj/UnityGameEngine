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

	BezierCurve wireArc;
	GameObject prefab;
	public float brickLength, brickHeight, brickDepth, wallLength, wallHeight, wallDepth;
	float topHeight;
	float leftPoint;
	GameObject parent;


	void Awake () {
		//initiations
		wireArc = this.gameObject.GetComponent<BezierCurve>();
	}

	public void SetAbutmentPositions(float pos) {
		leftPoint = pos;
	}


	public void BuildAbutment () {
		//position will be (0,0,0) anyway
		parent = new GameObject ("Abutment");

		//activated when Populate the Wall button is clicked
		CreatePrefabBrick();

		//cleaning the objects created before
		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
		for (int i = 0; i < bricks.Length; i++) {
			Destroy (bricks [i].gameObject);
		}

		if (wallDepth <= brickDepth) {
			throw new Exception ("Abutment depth cannot be less than the abutment brick depth.");
		} else {
			//create the left abutment
			DrawWall (new Vector3 ((leftPoint - wallLength + brickLength / 2), brickHeight / 2, (-wallDepth + brickDepth) / 2));
			//copy the left abutment over to the right side
			GameObject rightAbutment = Instantiate (parent) as GameObject;
			rightAbutment.transform.position = new Vector3 (wallLength + (wireArc.points [3].x - wireArc.points [0].x), 
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
			brick.transform.localScale = new Vector3 (brickLength, topHeight, brickDepth);
		}
		return brick;
	}


	void DrawHalfBrick(Vector3 position) {
		//creates a brick with half the length of the prefab with its center in the given position
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (brickLength / 2, topHeight, brickDepth);
		} else {
			brick.transform.localScale = new Vector3 (brickLength / 2, brickHeight, brickDepth);
		}
	}


	void DrawEndBrick (Vector3 position) {
		//creates a brick for the end of the row
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (Mathf.Abs(position.x - leftPoint) * 2, topHeight, brickDepth);
		}
		else {
			brick.transform.localScale = new Vector3 (Mathf.Abs(position.x - leftPoint) * 2, brickHeight, brickDepth);
		}
	}


	void DrawOddRow (Vector3 position) {
		//position.x starts with negative numbers; populate as long as whole bricks fit
		while (position.x < leftPoint) 
		{
			DrawBrick (position);
			position.x += brickLength;
		}
		//draw end brick if one can is needed
		if (position.x - leftPoint < brickLength/2) 
		{
			position.x = 3 * position.x / 2 - brickLength / 4- leftPoint / 2; 
			DrawEndBrick (position);
		}
	}


	void DrawEvenRow (Vector3 position) {
		//create half a brick first, and then repeat the process in DrawOddRow
		position.x = position.x - brickLength / 4;
		DrawHalfBrick (position);
		position.x += 3 * brickLength/ 4;
		while (position.x < leftPoint) 
		{
			DrawBrick (position);
			position.x += brickLength;
		}
		if (position.x - leftPoint < brickLength/2) 
		{
			position.x = 3 * position.x / 2 - brickLength / 4- leftPoint / 2; 
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
			Debug.Log ("drawing odd lane in the while loop");
			DrawOddLane (position);
			position.z += brickDepth;
			if ((wallDepth - 2 * position.z) < brickDepth) {
				//change the depth if whole brick depth does not fit and break
				position.z = wallDepth/4 + position.z/2 - brickDepth/4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z));
				DrawEvenLane (position);
				Debug.Log ("i am in the first if statement");
				return;
			} else if ((wallDepth - 2 * position.z) >= brickDepth) {
				DrawEvenLane (position);
				position.z += brickDepth;
			}
			if ((wallDepth - 2 * position.z) < brickDepth) {
				//change the depth if whole brick depth does not fit and break
				position.z = wallDepth / 4 + position.z / 2 - brickDepth / 4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z)); 
				DrawOddLane (position);
				Debug.Log ("i am in the second if statement");
				return;
			}
		}
	}
}