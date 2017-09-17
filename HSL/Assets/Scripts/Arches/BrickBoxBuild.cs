using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

//adapted from WallBuild script
public class BrickBoxBuild : MonoBehaviour {

	public float brickLength, brickHeight, brickDepth, wallLength, wallHeight, wallDepth;
	float topHeight;
	GameObject prefab;


	public void BrickWallBuild () {
		CreatePrefabBrick();
			//cleaning the bricks created before
			GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");
			for (int i = 0; i < bricks.Length; i++) {
				Destroy (bricks [i].gameObject);
			}

			if (wallDepth <= brickDepth) {
				prefab.transform.localScale = new Vector3 (prefab.transform.localScale.x,
					prefab.transform.localScale.y, wallDepth);
				DrawOddLane (new Vector3((-wallLength + brickLength) / 2, brickHeight / 2, 0));
			} else {
				DrawWall (new Vector3 ((-wallLength + brickLength) / 2, brickHeight / 2, (-wallDepth + brickDepth) / 2));
			}
		}

	void CreatePrefabBrick () {
			//creates a prefab with the given dimensions and color
			GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = new Vector3 (brickLength, brickHeight, brickDepth);
			PrefabUtility.CreatePrefab ("Assets/Resources/BoxBrick.prefab", brick);
			Destroy (brick);

			prefab = Resources.Load ("BoxBrick") as GameObject;
		prefab.GetComponent<Renderer> ().material = Resources.Load ("Brick") as Material;
		}


		void DrawWall (Vector3 position) {
			//build alternating Odd and Even Lanes while whole lanes can be built
			while ((wallDepth - 2 * position.z) >= brickDepth) {
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


		void DrawOddLane (Vector3 position) {
			topHeight = brickHeight;
			while ((position.y - brickHeight/2) < wallHeight) {
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
			while ((position.y - brickHeight/2) < wallHeight) {
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


		void DrawOddRow (Vector3 position) {
			//position.x starts with negative numbers; populate as long as whole bricks fit
			while ((wallLength - 2 * position.x) >= brickLength) {
				DrawBrick (position);
				position.x += brickLength;
			}
			//draw end brick if one can is needed
			if ((wallLength+brickLength)/2 > position.x) 
			{
				position.x = wallLength / 4 + position.x / 2 - brickLength / 4;
				DrawEndBrick (position);
			}
		}


		void DrawEvenRow (Vector3 position) {
			//create half a brick first, and then repeat the process in DrawOddRow
			position.x = position.x - brickLength / 4;
			DrawHalfBrick (position);
			position.x += 3 * brickLength/ 4;
			while ((wallLength - 2 * position.x) >= brickLength) {
				DrawBrick (position);
				position.x += brickLength;
			}
			if ((wallLength+brickLength)/2 > position.x) 
			{
				position.x = wallLength / 4 + position.x / 2 - brickLength / 4;
				DrawEndBrick (position);
			}
		}


		void DrawBrick(Vector3 position) {
			//creates a brick from the prefab with its center in the given position
			GameObject brick = Instantiate (prefab) as GameObject;
			brick.transform.position = position;
			//tag name needs to be defined (added into tags) before runtime
			brick.tag = "Brick"; 
			//if on last row, need to change the height, aka scale
			if (topHeight != brickHeight) {
				brick.transform.localScale = new Vector3 (brick.transform.localScale.x, topHeight,
					brick.transform.localScale.z);
			}
		}


		void DrawHalfBrick(Vector3 position) {
			//creates a brick with half the length of the prefab with its center in the given position
			GameObject brick = Instantiate (prefab) as GameObject;
			brick.transform.position = position;
			brick.tag = "Brick"; 
			if (topHeight != brickHeight) {
				brick.transform.localScale = new Vector3 (brickLength / 2,
					topHeight, prefab.transform.localScale.z);
			} else {
				brick.transform.localScale = new Vector3 (brickLength / 2,
					prefab.transform.localScale.y, prefab.transform.localScale.z);
			}
		}


		void DrawEndBrick (Vector3 position) {
			//creates a brick for the end of the row
			GameObject brick = Instantiate (prefab) as GameObject;
			brick.transform.position = position;
			brick.tag = "Brick"; 
			if (topHeight != brickHeight) {
				brick.transform.localScale = new Vector3 ((wallLength - 2 * position.x), 
					topHeight, prefab.transform.localScale.z);
			}
			else {
				brick.transform.localScale = new Vector3 ((wallLength - 2 * position.x), 
					prefab.transform.localScale.y, prefab.transform.localScale.z);
			}
		}
	}