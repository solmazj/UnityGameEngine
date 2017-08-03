using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialMesh : MonoBehaviour {

	public float haunchHeight = 4;
	public float haunchLength = 1;
	public void CreateMesh () {

		//check if the object that the script is attached to is an empty object
		if (GetComponent<MeshFilter> () == null) {
			gameObject.AddComponent<MeshFilter> ();
		}
		if (GetComponent<MeshRenderer> () == null) {
			gameObject.AddComponent<MeshRenderer> ();
		}

		//create the mesh
		Mesh mesh = new Mesh();
		mesh.name = "Trial";
	
		float freeSpan = this.gameObject.GetComponent<Corbel> ().freeSpan, brickLength = this.gameObject.GetComponent<Corbel> ().brickLength;
		float depth = this.gameObject.GetComponent<Corbel> ().brickDepth/2, brickHeight = this.gameObject.GetComponent<Corbel> ().brickHeight;
		float overhang = this.gameObject.GetComponent<Corbel> ().overhang, distance = freeSpan / 2 + brickLength + haunchLength;
//		int keepTrack = 1;
		int offset = 0;
		int j = 0;
		//works only if the haunch does not start right where the arch ends (*2 part)
		int numberOfTriangles = this.gameObject.GetComponent<Corbel> ().count * 4;
		Debug.Log ("number of triangles is " + numberOfTriangles);
		int numberOfVertices = numberOfTriangles * 3;
		Debug.Log ("number of vertices is " + numberOfVertices);
		//assigning vertices
		Vector3[] vertices = new Vector3[numberOfVertices];
		Vector3 middle = new Vector3 (distance, 0, -depth);
		for (int keepTrack = 1, i = 0; keepTrack <= numberOfTriangles; keepTrack++, i += 3) {
			vertices [i] = new Vector3 (distance, haunchHeight, -depth);
			vertices [i+1] = middle;
			if (keepTrack % 2 != 0) {
				if (keepTrack == 1) {
					j = 0;
				}
				else {
					j++;
				}
			vertices [i + 2] = new Vector3 (distance - haunchLength - j * overhang, offset * brickHeight, -depth);
			} else {
				offset++;
				vertices [i + 2] = new Vector3 (distance - haunchLength - j * overhang, offset * brickHeight, -depth);
			}
			middle = vertices [i + 2];
//			keepTrack++;
		}
		mesh.vertices = vertices;

		//assigning triangles
		int[] triangles = new int[numberOfVertices];

		for (int s = 0; s < numberOfVertices; s++) {
			triangles [s] = s;
		}

		mesh.triangles = triangles;


		//do lighting calcs
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		GetComponent<MeshFilter>().mesh = mesh;
	}
}
