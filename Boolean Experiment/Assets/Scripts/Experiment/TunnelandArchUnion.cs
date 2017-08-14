using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;

public class TunnelandArchUnion : MonoBehaviour {

	public void Union () {
		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("StructureBrick");
	
		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere(brick.transform.position, brick.transform.localScale.x);
		
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders [i].gameObject.transform.tag == "Tunnel") {

					csg.Brush = hitColliders[i].gameObject;
					csg.OperationType = CSG.Operation.Union;
					csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
					csg.useCustomMaterial = false; // Use the above material to fill cuts
					csg.hideGameObjects = false; // Hide target and brush objects after operation

					csg.keepSubmeshes = false; // Keep original submeshes and materials
					newOne = csg.PerformCSG();
					Destroy (csg.Target);
					csg.Target = newOne;
					newOne.name = "New";
				}
			}
		}
		Destroy(GameObject.Find("Tunnel"));
	}
}
