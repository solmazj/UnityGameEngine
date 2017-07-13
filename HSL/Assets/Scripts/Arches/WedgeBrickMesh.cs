﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class WedgeBrickMesh : MonoBehaviour {

	public float innerLength, outerLength, height, depth;
	public Material material;


	void Start () {
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
		vertices [0] = new Vector3 (0, 0, -inner);          //front left bottom
		vertices [1] = new Vector3 (0, 0, inner);			//front right bottom
		vertices [2] = new Vector3 (0, height, -outer);		//front left up
		vertices [3] = new Vector3 (0, height, outer);		//front right up
		vertices [4] = new Vector3 (depth, 0, -inner);		//back left bottom
		vertices [5] = new Vector3 (depth, 0, inner);		//back right bottom
		vertices [6] = new Vector3 (depth, height, -outer); //back left up
		vertices [7] = new Vector3 (depth, height, outer);	//back right up
		vertices [8] = new Vector3 (0, 0, inner);
		vertices [9] = new Vector3 (0, height, outer);
		vertices [10] = new Vector3 (depth, 0, inner);
		vertices [11] = new Vector3 (depth, height, outer);
		vertices [12] = new Vector3 (0, 0, -inner);
		vertices [13] = new Vector3 (0, height, -outer);
		vertices [14] = new Vector3 (depth, 0, -inner);
		vertices [15] = new Vector3 (depth, height, -outer);
		vertices [16] = new Vector3 (0, height, -outer);
		vertices [17] = new Vector3 (0, height, outer);
		vertices [18] = new Vector3 (depth, height, -outer);
		vertices [19] = new Vector3 (depth, height, outer);
		vertices [20] = new Vector3 (0, 0, -inner);
		vertices [21] = new Vector3 (0, 0, inner);
		vertices [22] = new Vector3 (depth, 0, -inner);
		vertices [23] = new Vector3 (depth, 0, inner);
		mesh.vertices = vertices;

		//assigning triangles
		int[] triangles = new int[] {
			3, 2, 0, 3, 0, 1, //front face
			5, 6, 7, 6, 5, 4,  //back face
			11, 9, 8, 11, 8, 10, //right face
			13, 15, 14, 13, 14, 12, //left face
			19, 18, 16, 19, 16, 17, //up face
			21, 20, 23, 20, 22, 23, //bottom face
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
