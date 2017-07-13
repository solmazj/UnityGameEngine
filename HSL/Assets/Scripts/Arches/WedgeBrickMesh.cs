using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class WedgeBrickMesh : MonoBehaviour {

	public float innerLength, outerLength, height, depth;
	public Material material;


	void Update () {
		Debug.Log ("I am being constantly updated");
		if (GetComponent<MeshFilter> () == null) {
			gameObject.AddComponent<MeshFilter> ();
		}
		if (GetComponent<MeshRenderer> () == null) {
			gameObject.AddComponent<MeshRenderer> ();
		}

		Mesh mesh = new Mesh();
		mesh.name = "WedgeBrick";

		float inner = innerLength / 2;
		float outer = outerLength / 2;

		//assigning vertices
		Vector3[] vertices = new Vector3[24];
		vertices [0] = new Vector3 (-inner, 0, 0);          //front left bottom
		vertices [1] = new Vector3 (inner, 0, 0);			//front right bottom
		vertices [2] = new Vector3 (-outer, height, 0);		//front left up
		vertices [3] = new Vector3 (outer, height, 0);		//front right up
		vertices [4] = new Vector3 (-inner, 0, depth);		//back left bottom
		vertices [5] = new Vector3 (inner, 0, depth);		//back right bottom
		vertices [6] = new Vector3 (-outer, height, depth); //back left up
		vertices [7] = new Vector3 (outer, height, depth);	//back right up
		vertices [8] = new Vector3 (inner, 0, 0);
		vertices [9] = new Vector3 (outer, height, 0);
		vertices [10] = new Vector3 (inner, 0, depth);
		vertices [11] = new Vector3 (outer, height, depth);
		vertices [12] = new Vector3 (-inner, 0, 0);
		vertices [13] = new Vector3 (-outer, height, 0);
		vertices [14] = new Vector3 (-inner, 0, depth);
		vertices [15] = new Vector3 (-outer, height, depth);
		vertices [16] = new Vector3 (-outer, height, 0);
		vertices [17] = new Vector3 (outer, height, 0);
		vertices [18] = new Vector3 (-outer, height, depth);
		vertices [19] = new Vector3 (outer, height, depth);
		vertices [20] = new Vector3 (-inner, 0, 0);
		vertices [21] = new Vector3 (inner, 0, 0);
		vertices [22] = new Vector3 (-inner, 0, depth);
		vertices [23] = new Vector3 (inner, 0, depth);
		mesh.vertices = vertices;

		//assigning triangles
		int[] triangles = new int[] {
			0, 2, 3, 1, 0, 3, //front face
			7, 6, 5, 4, 5, 6,  //back face
			8, 9, 11, 10, 8, 11, //right face
			14, 15, 13, 12, 14, 13, //left face
			16, 18, 19, 17, 16, 19, //up face
			23, 20, 21, 23, 22, 20, //bottom face
		};
		mesh.triangles = triangles;

		//assigning uv for textures
		Vector2 uv0 = new Vector2 (0, 0);
		Vector2 uv1 = new Vector2 (1f, 0);
		Vector2 uv2 = new Vector2 (0, 1f);
		Vector2 uv3 = new Vector2 (1f, 1f);
		Vector2[] uvs = new Vector2 [] {
			uv0, uv1, uv2, uv3, //front face
			uv1, uv0, uv3, uv2, //back face
			uv0, uv2, uv1, uv3, //right face
			uv1, uv3, uv0, uv2, //left face
			uv0, uv1, uv2, uv3, //up face
			uv1, uv0, uv3, uv2, //bottom face
		};
		mesh.uv = uvs;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		GetComponent<MeshRenderer> ().material = material;
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
