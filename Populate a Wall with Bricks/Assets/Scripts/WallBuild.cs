using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//attaches to the brick
public class WallBuild : MonoBehaviour {

	public GameObject wall;
	GameObject modeledBrick;
	GameObject prefab;
	float brickLength;
	float brickHeight;
	float brickDepth;
	float wallLength;
	float wallHeight;
	float wallDepth;
	List<GameObject> goList;
	float topHeight;

	void Awake () {
		//initiations
		modeledBrick = this.gameObject;
		goList = new List<GameObject>();
	}
		
	public void CreatePrefabBrick () {
		//activated when push on Brick Ready button
		//creates a gameobject called Cube with the center at the global origin
		GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = modeledBrick.transform.localScale;
		PrefabUtility.CreatePrefab ("Assets/Resources/createdBrick.prefab", brick);
		brick.SetActive(false);
		Material mat = modeledBrick.GetComponent<Renderer> ().material;
		prefab = Resources.Load ("createdBrick") as GameObject;
		//makes a prefab out of this Cube and changes its color
		prefab.GetComponent<Renderer> ().material = mat;
	}

	public void PopulateWallWithCreatedBrick () {
		//activated when Populate the Wall button is clicked
		//global variables assigned
		brickLength = modeledBrick.transform.localScale.x;
		brickHeight = modeledBrick.transform.localScale.y;
		brickDepth = modeledBrick.transform.localScale.z;
		wallLength = wall.transform.localScale.x;
		wallHeight = wall.transform.localScale.y;
		wallDepth = wall.transform.localScale.z;
		//topHeight = brickHeight;
		if (wallDepth <= brickDepth) {
			prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x,
				prefab.transform.localScale.y, wallDepth);
			DrawOddLane (new Vector3((-wallLength + brickLength) / 2, brickHeight / 2, 0));
		} else {
			DrawRows (new Vector3 ((-wallLength + brickLength) / 2, brickHeight / 2, (-wallDepth + brickDepth) / 2));
		}
		wall.SetActive (false);
	}

	GameObject DrawBrick(Vector3 position) {
		//creates a brick from the prefab with its center in the given position
		GameObject brick = Instantiate (prefab) as GameObject;
		brick.transform.position = position;
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (brick.transform.localScale.x, topHeight,
				brick.transform.localScale.z);
		}
		return brick;
	}

	GameObject DrawHalfBrick(Vector3 position) {
		//creates a brick with half the length of the prefab with its center in the given position
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 (brickLength / 2,
				topHeight, prefab.transform.localScale.z);
		} else {
			brick.transform.localScale = new Vector3 (brickLength / 2,
				prefab.transform.localScale.y, prefab.transform.localScale.z);
		}
		return brick;
	}

	GameObject DrawEndBrick (Vector3 position) {
		GameObject brick = DrawBrick (position);
		if (topHeight != brickHeight) {
			brick.transform.localScale = new Vector3 ((wallLength - 2 * position.x), 
				topHeight, prefab.transform.localScale.z);
		}
			else {
				brick.transform.localScale = new Vector3 ((wallLength - 2 * position.x), 
				prefab.transform.localScale.y, prefab.transform.localScale.z);
			}
		return brick;
	}

	void DrawOddRow (Vector3 position) {
		//ok when position.x is negative, not true when the position.x is too big positive number, so ok
		while ((wallLength - 2 * position.x) >= brickLength) 
		{
			goList.Add (DrawBrick (position));
			position.x += brickLength;
		}
		if ((wallLength+brickLength)/2 > position.x) 
		{
			position.x = wallLength / 4 + position.x / 2 - brickLength / 4;
			goList.Add (DrawEndBrick (position));
		}
	}

	void DrawEvenRow (Vector3 position) {
		position.x = position.x - brickLength / 4;
		goList.Add (DrawHalfBrick (position));
		//move half a brick, because just created a half a brick
		position.x += 3 * brickLength/ 4;
		while ((wallLength - 2 * position.x) >= brickLength) 
		{
			goList.Add (DrawBrick (position));
			position.x += brickLength;
		}
		if ((wallLength+brickLength)/2 > position.x) 
		{
			position.x = wallLength / 4 + position.x / 2 - brickLength / 4;
			goList.Add (DrawEndBrick (position));
		}
	}

	void DrawOddLane (Vector3 position) {
		topHeight = brickHeight;
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				//draw a smaller (odd) brick and break
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				topHeight = 2 * (wallHeight-position.y);
				DrawOddRow (position);
				return;
			} else {
				DrawOddRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				
				//draw a smaller (even) brick and break
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
		topHeight = brickHeight;
		while ((position.y - brickHeight/2) < wallHeight)
		{
			if (2 * (wallHeight - position.y) < brickHeight) {
				//draw a smaller (even) brick and break
				position.y = wallHeight/2 + position.y/2 - brickHeight/4;
				topHeight = 2 * (wallHeight-position.y);
				DrawEvenRow (position);
				return;
			} else {
				DrawEvenRow (position);
				position.y += brickHeight;
			}
			if (Mathf.Abs(2 * (wallHeight - position.y)) < brickHeight) {
				//draw a smaller (odd) brick and break
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

	void DrawRows (Vector3 position) {
		while ((wallDepth - 2 * position.z) >= brickDepth)
		{
				DrawOddLane (position);
				position.z += brickDepth;
			if ((wallDepth - 2 * position.z) < brickDepth) {
				position.z = wallDepth/4 + position.z/2 - brickDepth/4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z));
				DrawEvenLane (position);
				return;
			} else if ((wallDepth - 2 * position.z) >= brickDepth) {
				DrawEvenLane (position);
				position.z += brickDepth;
			}
			if ((wallDepth - 2 * position.z) < brickDepth) {
				position.z = wallDepth / 4 + position.z / 2 - brickDepth / 4;
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x, 
					prefab.transform.localScale.y, (wallDepth - 2 * position.z)); 
				DrawOddLane (position);
				return;
			}
		}
	}
}