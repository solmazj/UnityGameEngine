using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : MonoBehaviour {
	
	//only one row of bricks;
	//anchor is a bad name
	public static void wallBuild (float hor, float ver, GameObject anchor) {

		GameObject prefab = Resources.Load ("brickRed") as GameObject;
		for (int i = 0; i < ver; i++) {
			float offset = i % 2;
			for (int j = 0; j < hor; j++) {
				GameObject brick = Instantiate (prefab) as GameObject;
				brick.transform.position = new Vector3 (anchor.transform.position.x + j * 2 + offset,
					anchor.transform.position.y + i, anchor.transform.position.z);
			}
		}
	}

	//multiple rows of bricks
	public static void wallBuild (float hor, float ver, float depth, GameObject anchor) {
		GameObject prefab = Resources.Load ("brickRed") as GameObject;
		for (int k = 0; k < depth; k++) {
			for (int i = 0; i < ver; i++) {
				//better but not ideal, still need boundary conditions
				float offset = (k % 2 + i % 2) % 2;
				for (int j = 0; j < hor; j++) {
					GameObject brick = Instantiate (prefab) as GameObject;
					brick.transform.position = new Vector3 (anchor.transform.position.x + j * 2 + offset,
						anchor.transform.position.y + i, anchor.transform.position.z + k);
				}
			}
		}
	} 
}