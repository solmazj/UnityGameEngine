using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialMesh : MonoBehaviour {

	public float haunchHeight = 4;
	public float haunchLength = 1;
	public string materialName = "Stone";
	public void CreateMesh () {

		float yPos = this.gameObject.GetComponent<Corbel> ().yPos;

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
		GameObject[] corbel = GameObject.FindGameObjectsWithTag ("Corbel");
		float depth;
		if (corbel.Length > 1) {
			depth = this.gameObject.GetComponent<Corbel> ().vaultDepth / 2;
		} else {
			depth = this.gameObject.GetComponent<Corbel> ().brickDepth / 2;
		}
		float brickHeight = this.gameObject.GetComponent<Corbel> ().brickHeight;
		float overhang = this.gameObject.GetComponent<Corbel> ().overhang, distance = freeSpan / 2 + brickLength + haunchLength;
//		int keepTrack = 1;
		int offset = 0;
		int j = 0; int i = 0;
		//+12 is manually input and accounts for
		int numberOfTriangles = this.gameObject.GetComponent<Corbel> ().count * 8 + 22;
//		numberOfTriangles = 8;
		int numberOfVertices = numberOfTriangles * 3;
		//assigning vertices
		Vector3[] vertices = new Vector3[numberOfVertices];
		Vector3 middle = new Vector3 (distance, yPos, -depth);
		Vector3 middleOpp = new Vector3 (-distance, yPos, depth);
		Vector3 middleFront = new Vector3 (-distance, yPos, -depth);
		Vector3 middleBack = new Vector3 (distance, yPos, depth);
		//numberOfTriangles is for 2 sides, so by /2 we take only one side
//		
		for (int keepTrack = 1; keepTrack <= this.gameObject.GetComponent<Corbel> ().count * 2; keepTrack++) {
			vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
			vertices [i + 3] = new Vector3 (-distance, yPos + haunchHeight, depth);
			vertices [i + 8] = new Vector3 (-distance, yPos + haunchHeight, -depth);
			vertices [i + 11] = new Vector3 (distance, yPos + haunchHeight, depth);
			vertices [i + 1] = middle;
			vertices [i + 4] = middleOpp;
			vertices [i + 7] = middleFront;
			vertices [i + 10] = middleBack;
			if (keepTrack % 2 != 0) {
				if (keepTrack == 1) {
					j = 0;
				}
				else {
					j++;
				}
				vertices [i + 2] = new Vector3 (distance - haunchLength - j * overhang, yPos + offset * brickHeight, -depth);
				vertices [i + 5] = new Vector3 (-(distance - haunchLength - j * overhang), yPos + offset * brickHeight, depth);
				vertices [i + 6] = new Vector3 (-(distance - haunchLength - j * overhang), yPos + offset * brickHeight, -depth);
				vertices [i + 9] = new Vector3 (distance - haunchLength - j * overhang, yPos + offset * brickHeight, depth);
			} else {
				offset++;
				vertices [i + 2] = new Vector3 (distance - haunchLength - j * overhang, yPos + offset * brickHeight, -depth);
				vertices [i + 5] = new Vector3 (-(distance - haunchLength - j * overhang), yPos + offset * brickHeight, depth);
				vertices [i + 6] = new Vector3 (-(distance - haunchLength - j * overhang), yPos + offset * brickHeight, -depth);
				vertices [i + 9] = new Vector3 (distance - haunchLength - j * overhang, yPos + offset * brickHeight, depth);
			}
			middle = vertices [i + 2];
			middleOpp = vertices [i + 5];
			middleFront = vertices [i + 6];
			middleBack = vertices [i + 9];
//			keepTrack++;
			i += 12;
		}

		//triangle 5
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 3] = new Vector3 (-distance, yPos + haunchHeight, depth);
		vertices [i + 8] =new Vector3 (-distance, yPos + haunchHeight, -depth);
		vertices [i + 11] =new Vector3 (distance, yPos + haunchHeight, depth);

		vertices [i + 1] = middle;
		vertices [i + 4] = middleOpp;
		vertices [i + 7] = middleFront;
		vertices [i + 10] = middleBack;

		vertices [i + 2] = new Vector3 (brickLength/2, yPos + this.gameObject.GetComponent<Corbel> ().count * brickHeight, -depth);
		vertices [i + 5] = new Vector3 (-brickLength/2, yPos + this.gameObject.GetComponent<Corbel> ().count * brickHeight, depth);
		vertices [i + 6] = new Vector3 (-brickLength/2, yPos + this.gameObject.GetComponent<Corbel> ().count * brickHeight, -depth);
		vertices [i + 9] = new Vector3 (brickLength/2, yPos + this.gameObject.GetComponent<Corbel> ().count * brickHeight, depth);
		middle = vertices [i + 2];
		middleOpp = vertices [i + 5];
		middleFront = vertices [i + 6];
		middleBack = vertices [i + 9];
		i += 12;
		//triangle 6
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 3] = new Vector3 (-distance, yPos + haunchHeight, depth);
		vertices [i + 8] = new Vector3 (-distance, yPos +  haunchHeight, -depth);
		vertices [i + 11] = new Vector3 (distance, yPos + haunchHeight, depth);
		vertices [i + 1] = middle;
		vertices [i + 4] = middleOpp;
		vertices [i + 7] = middleFront;
		vertices [i + 10] = middleBack;
		vertices [i + 2] = new Vector3 (brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, -depth);
		vertices [i + 5] = new Vector3 (-brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, depth);
		vertices [i + 6] = new Vector3 (-brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, -depth);
		vertices [i + 9] = new Vector3 (brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, depth);
		middle = vertices [i + 2];
		middleOpp = vertices [i + 5];
		middleFront = vertices [i + 6];
		middleBack = vertices [i + 9];
		i += 12;

		//triangle 7 smaller
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 3] = new Vector3 (-distance, yPos + haunchHeight, depth);
		vertices [i+1] = middle;
		vertices [i + 4] = middleOpp;
		vertices [i + 2] = new Vector3 (- brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, -depth);
		vertices [i + 5] = new Vector3 (brickLength/2, yPos + (this.gameObject.GetComponent<Corbel> ().count + 1) * brickHeight, depth);
		middle = vertices [i + 2];
		middleOpp = vertices [i + 5];
		i += 6;

		//triangle 7 bigger
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 3] = new Vector3 (-distance, yPos + haunchHeight, depth);
		vertices [i+1] = middle;
		vertices [i + 4] = middleOpp;
		vertices [i + 2] = new Vector3 (-distance, yPos + haunchHeight, -depth);
		vertices [i + 5] = new Vector3 (distance, yPos + haunchHeight,depth);
		i += 6;

//		middle = vertices [i + 1];
//		middleOpp = vertices [i + 4];
//		i += 6;

		//right side
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, depth);
		vertices [i + 1] = new Vector3 (distance, yPos, depth);
		vertices [i + 2] = new Vector3 (distance, yPos, -depth);
		vertices [i + 3] = vertices [i + 2];
		vertices [i + 4] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 5] = vertices[i];
		i += 6;

		//right bottom
		vertices [i] = new Vector3 (distance, yPos, depth);
		vertices [i + 1] = new Vector3 (distance - haunchLength, yPos, depth);
		vertices [i + 2] = new Vector3 (distance - haunchLength, yPos, -depth);
		vertices [i + 3] = vertices [i + 2];
		vertices [i + 4] = new Vector3 (distance, yPos, -depth);
		vertices [i + 5] = vertices[i];
		i += 6;


		//left side
		vertices [i] = new Vector3 (-distance, yPos + haunchHeight, -depth);
		vertices [i + 1] = new Vector3 (-distance, yPos, -depth);
		vertices [i + 2] = new Vector3 (-distance, yPos, depth);
		vertices [i + 3] = vertices [i + 2];
		vertices [i + 4] = new Vector3 (-distance, yPos + haunchHeight, depth);
		vertices [i + 5] = vertices[i];
		i += 6;

		//left bottom
		vertices [i] = new Vector3 (-distance, yPos, -depth);
		vertices [i + 1] = new Vector3 (-distance + haunchLength, yPos, -depth);
		vertices [i + 2] = new Vector3 (-distance + haunchLength, yPos, depth);
		vertices [i + 3] = vertices [i + 2];
		vertices [i + 4] = new Vector3 (-distance, yPos, depth);
		vertices [i + 5] = vertices[i];
		i += 6;


		//top side
		vertices [i] = new Vector3 (distance, yPos + haunchHeight, -depth);
		vertices [i + 1] = new Vector3 (-distance, yPos + haunchHeight, -depth);
		vertices [i + 2] = new Vector3 (-distance,  yPos + haunchHeight, depth);
		vertices [i + 3] = vertices [i + 2];
		vertices [i + 4] = new Vector3 (distance, yPos + haunchHeight, depth);
		vertices [i + 5] = vertices[i];
//		i += 6;

		mesh.vertices = vertices;

		//assigning triangles
		int[] triangles = new int[numberOfVertices];

		for (int s = 0; s < numberOfVertices; s++) {
			triangles [s] = s;
		}

		mesh.triangles = triangles;
	




		//assigning uv for textures
		Vector2 uv = new Vector2 (0.5f, 0.5f);
		Vector2[] uvs = new Vector2 [numberOfVertices];
		for (int l = 0; l < numberOfVertices; l++) {
			uvs [l] = uv;
		}
		mesh.uv = uvs;


		//do lighting calcs
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshRenderer> ().material =  Resources.Load (materialName) as Material;
	}
}
