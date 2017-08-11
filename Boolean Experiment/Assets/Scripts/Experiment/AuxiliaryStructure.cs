using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;

public class AuxiliaryStructure : MonoBehaviour {

	public float wallLength, wallHeight, wallDepth;

	public void SolidBox () {
		GameObject box =  GameObject.CreatePrimitive(PrimitiveType.Cube);
		box.name = ("SolidWall");

		box.transform.localScale = new Vector3 (wallLength, wallHeight, wallDepth);

		box.transform.position = new Vector3 (0, wallHeight / 2, 0);
	}


	public void Difference () {
		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG();
	
		csg.Target = GameObject.Find("SolidWall");
		GameObject newOne;
		GameObject[] tunnels = GameObject.FindGameObjectsWithTag ("Tunnel");
		foreach (GameObject tunnel in tunnels) {
			csg.Brush = tunnel;
			csg.OperationType = CSG.Operation.Subtract;
			csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
			csg.useCustomMaterial = false; // Use the above material to fill cuts
			csg.hideGameObjects = false; // Hide target and brush objects after operation
			csg.keepSubmeshes = true; // Keep original submeshes and materials
			newOne = csg.PerformCSG();
			Destroy (csg.Target);
			csg.Target = newOne;
			newOne.name = "SolidWall";
		}
		Destroy (GameObject.Find ("Tunnel"));

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("StructureBrick");
		foreach (GameObject tunnel in bricks) {
			csg.Brush = tunnel;
			csg.OperationType = CSG.Operation.Subtract;
			csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
			csg.useCustomMaterial = false; // Use the above material to fill cuts
			csg.hideGameObjects = false; // Hide target and brush objects after operation
			csg.keepSubmeshes = true; // Keep original submeshes and materials
			newOne = csg.PerformCSG();
			Destroy (csg.Target);
			csg.Target = newOne;
			newOne.name = "SolidWall";
		}
	}
}
