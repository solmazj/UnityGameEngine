using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MeshMakerNamespace;

public class ExceptionTest : MonoBehaviour {

	public void TestException () { 
		throw new Exception ("I am throwing an exception");	
	}

	public void TestDebug () { 
		Debug.Log("I am a debug");	
	}


	public void Difference () {
		CSG.EPSILON = 1e-5f; // Adjustable epsilon value
		CSG csg = new CSG();
		csg.Brush = GameObject.Find("Sphere");
		csg.Target = GameObject.Find("Cube");
		csg.OperationType = CSG.Operation.Subtract;
		csg.customMaterial = new Material(Shader.Find("Standard")); // Custom material
		csg.useCustomMaterial = false; // Use the above material to fill cuts
		csg.hideGameObjects = true; // Hide target and brush objects after operation
		csg.keepSubmeshes = true; // Keep original submeshes and materials
		GameObject newObject = csg.PerformCSG();
	}
}
