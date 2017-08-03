using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Corbel : MonoBehaviour {

	public float brickLength, brickHeight, brickDepth, freeSpan, overhang, vaultDepth;
	[HideInInspector]
	public int count;
	GameObject prefab;
	float yPos = 0;

	void Start () {
		if (this.gameObject.GetComponent<Abutments> () != null) {
			this.gameObject.GetComponent<Abutments> ().SetAbutmentPositions (-freeSpan / 2);
		}
	}

	public void SetSpringLine () {
		yPos = this.gameObject.GetComponent<Abutments> ().wallHeight;
	}


	public void CorbelledArch () {
		CleanUp ();
		Corbelled (0);
	}


	public void CorbelledVault () { 
		CleanUp ();

		for (int i = 0; i < vaultDepth/brickDepth; i++) {
			Corbelled ((-vaultDepth + brickDepth) / 2 + i * brickDepth); 
		}
	}


	void CleanUp () {
		//cleaning the objects created before
		GameObject[] Corbelled = GameObject.FindGameObjectsWithTag ("Corbel");
		for (int i = 0; i < Corbelled.Length; i++) {
			Destroy (Corbelled [i].gameObject);
		}

		CreatePrefab ();
	}


	void Corbelled (float z) {
		count = 0;
		//creating a parent object. Position will be (0,0,0)
		GameObject parent = new GameObject("Corbelled Arch");
		parent.transform.tag = "Corbel";

		float initialX = (freeSpan + brickLength) / 2, initialY = yPos + brickHeight / 2;

		int j = 0, k = 0;
		while (2 * (initialX - k * overhang) > brickLength) {
			GameObject brickRight = Instantiate (prefab) as GameObject;
			brickRight.transform.position = new Vector3 (initialX - k * overhang, initialY + j * brickHeight, z);
			brickRight.transform.parent = parent.transform;
			brickRight.transform.tag = "Brick";

			GameObject brickLeft = Instantiate (prefab) as GameObject;
			brickLeft.transform.position = new Vector3 (-(initialX - k * overhang), initialY + j * brickHeight, z);
			brickLeft.transform.parent = parent.transform;
			brickLeft.transform.tag = "Brick";

			j++; k++; count++;
		}
		GameObject brick = Instantiate (prefab) as GameObject;
		brick.transform.position = new Vector3 (0, initialY + j * brickHeight, z);
		brick.transform.parent = parent.transform;
		brick.transform.tag = "Brick";
	}
		

	void CreatePrefab () {
		//creates a prefab with the given dimensions and color
		GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = new Vector3 (brickLength, brickHeight, brickDepth);
		PrefabUtility.CreatePrefab ("Assets/Resources/CorbelBrick.prefab", brick);
		Destroy (brick);
		/*because it is a prefab, if the color of the modeled brick changes, the prefab color
		changes too even if this method is not called (in this case, even if button is not pressed*/
		prefab = Resources.Load ("CorbelBrick") as GameObject;
		prefab.GetComponent<Renderer> ().material = Resources.Load ("Brick") as Material;
	}
}