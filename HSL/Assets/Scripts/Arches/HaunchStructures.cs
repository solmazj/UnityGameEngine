using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshMakerNamespace;

public class HaunchStructures : MonoBehaviour {

	public float brickLength, brickHeight, brickDepth, wallLength, wallHeight, wallDepth;

	//builds a solid box using the dimensions input in the hierarchy
	public void SolidBox () {
		GameObject box =  GameObject.CreatePrimitive(PrimitiveType.Cube);
		box.name = ("SolidWall");

		box.transform.localScale = new Vector3 (wallLength, wallHeight, wallDepth);

		box.transform.position = new Vector3 (0, wallHeight / 2, 0);
	}


	//performs boolean difference between the solid box created above and an existing (corbel or circular) arch 
	public void SolidDifference () {
		CSG.EPSILON = 1e-5f; 
		CSG csg = new CSG();
	
		//first performs the difference between the tunnel
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

		//and then performs it with the structure brick itself 
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


	//fist do boolean difference with the tunnel
	public void Tunnel () {

		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG ();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");

		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere (brick.transform.position, brick.transform.localScale.x);
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
					Destroy (csg.Brush);
					csg.Target = newOne;
					newOne.name = "NewBrick";
					newOne.tag = "Brick";
				}
			}
		}
	}


	//then boolean difference with the arch bricks
	public void Bricks () {

		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG ();

		GameObject[] bricks = GameObject.FindGameObjectsWithTag ("Brick");

		foreach (GameObject brick in bricks) {
			csg.Target = brick;
			GameObject newOne;
			Collider[] hitColliders = Physics.OverlapSphere (brick.transform.position, GameObject.Find("Circular").GetComponent<ArchBuild>().brickDepth);
			for (int i = 0; i < hitColliders.Length; i++) {
				if (hitColliders [i].gameObject.transform.tag == "StructureBrick") {

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
					newOne.tag = "Brick";
				}
			}
		}
	}
}
