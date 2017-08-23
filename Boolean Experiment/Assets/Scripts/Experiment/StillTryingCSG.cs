using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;


public class StillTryingCSG : MonoBehaviour {

	public void Tunnel () {

		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG ();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("StructureBrick");

		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere (brick.transform.position, GameObject.Find("Circular").GetComponent<ArchBuild>().brickDepth);
			for (int i = 0; i < hitColliders.Length; i++) {
				if (hitColliders [i].gameObject.transform.tag == "Tunnel") {

					csg.Brush = hitColliders [i].gameObject;
					csg.OperationType = CSG.Operation.Subtract;
					csg.customMaterial = new Material (Shader.Find ("Standard")); // Custom material
					csg.useCustomMaterial = false; // Use the above material to fill cuts
					csg.hideGameObjects = false; // Hide target and brush objects after operation
					csg.keepSubmeshes = true; // Keep original submeshes and materials
					newOne = csg.PerformCSG ();
					Destroy (csg.Target);
					csg.Target = newOne;
					newOne.name = "NewBrick";
					newOne.tag = "StructureBrick";
					newOne.transform.parent = GameObject.Find ("Vault Structure").transform;
				}
			}
		}
	}


		
	public void Bricks () {

		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG ();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("StructureBrick");

		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere (brick.transform.position, GameObject.Find("Circular").GetComponent<ArchBuild>().brickDepth);
			for (int i = 0; i < hitColliders.Length; i++) {
				if (hitColliders [i].gameObject.transform.tag == "Brick") {

					csg.Brush = hitColliders [i].gameObject;
					csg.OperationType = CSG.Operation.Subtract;
					csg.customMaterial = new Material (Shader.Find ("Standard")); // Custom material
					csg.useCustomMaterial = false; // Use the above material to fill cuts
					csg.hideGameObjects = false; // Hide target and brush objects after operation
					csg.keepSubmeshes = true; // Keep original submeshes and materials
					newOne = csg.PerformCSG ();
					Destroy (csg.Target);
					csg.Target = newOne;
					newOne.name = "NewBrick";
					newOne.tag = "StructureBrick";
					newOne.transform.parent = GameObject.Find ("Vault Structure").transform;
				}
			}
		}
	}
}
