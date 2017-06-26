using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attaches to the wall
public class AnchorObject3D : MonoBehaviour 
{

	private GameObject wall;
	private GameObject anchorObject;
	float xScale;
	float yScale;
	float zScale;

	void Awake () 
	{
		wall = this.gameObject;
		xScale = wall.transform.localScale.x;
		yScale = wall.transform.localScale.y;
		zScale = wall.transform.localScale.z;

		anchorObject = new GameObject ();
		//position is at the lower right corner of the wall;
		//because it rises from the ground, we make y=0.5f (the center of the first brick)
		//constants 2, 0.5f have to do with the dimensions of the brick I'm using rn
		anchorObject.transform.position = new Vector3(-xScale/2 + 1, 0.5f, -zScale/2 + 0.5f);
	}

	void Start () 
	{
		WallBuilding.WallBuild (xScale/2, yScale, zScale, anchorObject);
		wall.SetActive (false);
	}
}
