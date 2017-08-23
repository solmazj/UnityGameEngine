using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Corbel : MonoBehaviour {

	//these parameters are entered in the Inspector; but will need to make them parameters in a dropdown menu
	//or something if the end-product needs to be a built game (because there will be no Inspector then)
	public float brickLength, brickHeight, brickDepth, freeSpan, overhang, vaultDepth;
	//this piece of code makes proceeding variable public but invisible in the Inspector
	[HideInInspector]
	public int count;
	//initially yPos = 0 because cannot use unassigned variable, but it's checked and modified as necessary later
	[HideInInspector]
	public float yPos = 0;

	//the creation of tunnel makes it hard to create a structure as a stand-alone, the tunnel is there to be used in 
	//boolean difference operations with solid walls (haunches) and brick walls (insertion of an arch into a wall)
	CorbelTunnel tunnel;
	GameObject prefab;


	void Awake () {
		//attach CorbelTunnel script to the corbel object, to later call Tunnel on the object
		tunnel = this.gameObject.AddComponent<CorbelTunnel> ();
	}


	void Start () {
		//this piece of code checks sets the abutments' position to where the arch ends; 
		//if statement is there to prevent any errors if the Abutment script is not attached  
		if (this.gameObject.GetComponent<Abutments> () != null) {
			this.gameObject.GetComponent<Abutments> ().SetAbutmentPositions (-freeSpan / 2);
		}
	}


	//this method is called from the Abutments button to set the starting height of the arch (which is equal to the height of abutments)
	public void SetSpringLine () {
		yPos = this.gameObject.GetComponent<Abutments> ().wallHeight;
	}


	public void CorbelledArch () {
		//do preliminary clean-up
		Preliminary ();
		//build a corbelled arch that is centered around 0 in z-axis
		Corbelled (0);
		//build the tunnel
		tunnel.Tunnel();
	}

	
	public void CorbelledVault () { 
		//do preliminary clean-up
		Preliminary ();
		//how many arches is there in the vault
		float division = vaultDepth / brickDepth;
		//check if the vault can be built by integer number of arches built one after another;
		//the exception does not appear in built games, only the proceeding code will not be executed.
		if(!Mathf.Approximately(division-Mathf.Round(division),0)) {
			throw new Exception ("Vault depth has to be a multiple of brick depth. Check the input.");
		}

		//build separate arches at different z locations that will make up the vault, start from negative z and by adding brickdepths move to positive z locations
		for (int i = 0; i < division; i++) {
			//the vault is still centered around 0 in z-axis; go -half vaultdepth (to reach the end of the vault) move half brick depth (arch's center) and build an arch;
			//move over another brickdepth and build another one
			Corbelled ((-vaultDepth + brickDepth) / 2 + i * brickDepth); 
		}
		//build the tunnel
		tunnel.Tunnel();
	}


	void Preliminary () {
		//cleaning corbelled arches created before; this code does not clean existing circular arches;
		//might need to modify this code to make it clean every previously existing structure in the scene.
		GameObject[] Corbelled = GameObject.FindGameObjectsWithTag ("Corbel");
		for (int i = 0; i < Corbelled.Length; i++) {
			Destroy (Corbelled [i].gameObject);
		}
		//destroys Tunnel associated with an arch (can also destroy circular's tunnel, will destroy the tunnel which is on top of the Hierarchy)
		Destroy(GameObject.Find("Tunnel"));

		//creating prefab from the input parameters
		CreatePrefab ();
	}


	void CreatePrefab () {
		//creates a prefab with the given dimensions and color
		GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = new Vector3 (brickLength, brickHeight, brickDepth);
		//make a prefab in the given directory with the specified name from the brick object created above
		PrefabUtility.CreatePrefab ("Assets/Resources/CorbelBrick.prefab", brick);
		//now destroy the brick (otherwise will appear in the game, because all created objects appear in the game)
		Destroy (brick);
		//load the created prefab as a global variable prefab to be used in arch construction
		prefab = Resources.Load ("CorbelBrick") as GameObject;
		//assign a material from the Resources folder with the name inside "" to the prefab
		//any changes made to the loaded prefab are automatically applied the prefab in the Resources folder (the ACTUAL prefab)
		prefab.GetComponent<Renderer> ().material = Resources.Load ("Stone") as Material;
	}


	//input z is the center of the arch along the z-axis
	void Corbelled (float z) {
		//public variable count will be accessed by the CorbelTunnel script, it signifies how many pairs of bricks
		//are built before the arch reaches its single final brick at the top.
		count = 0;
		//creating a parent object. Position will be (0,0,0)
		GameObject parent = new GameObject("Corbelled Arch");
		//tag it so the preliminary cleaning above can then be performed on corbelled arches
		parent.transform.tag = "Corbel";

		//the arch is symmetrical around y-axis, and these are the positions of the right brick of the bottom pair of bricks
		float initialX = (freeSpan + brickLength) / 2, initialY = yPos + brickHeight / 2;
		//j is used for x-positon, k is used for y-position 
		int j = 0, k = 0;
		//as long as the distance between a pair of bricks is bigger than one brick, do following;
		//if it is equal to or less than one brick, that means the pair of bricks is already touching each other 
		while ((initialX - j * overhang) > (brickLength/2)) {
			//build a brick on the right side
			GameObject brickRight = Instantiate (prefab) as GameObject;
			brickRight.transform.position = new Vector3 (initialX - j * overhang, initialY + k * brickHeight, z);
			brickRight.transform.parent = parent.transform;
			brickRight.transform.tag = "StructureBrick";
			//build a brick on the left side
			GameObject brickLeft = Instantiate (prefab) as GameObject;
			brickLeft.transform.position = new Vector3 (-(initialX - j * overhang), initialY + k * brickHeight, z);
			brickLeft.transform.parent = parent.transform;
			brickLeft.transform.tag = "StructureBrick";

			//increase j by moving bricks closer together (making a corbel), increase k to go higher, and increase count
			j++; k++; count++;
		}

		//creating one single brick at the top, to completely close the gap, because pairs of bricks are designed not to touch each other.
		GameObject brick = Instantiate (prefab) as GameObject;
		brick.transform.position = new Vector3 (0, initialY + k * brickHeight, z);
		brick.transform.parent = parent.transform;
		brick.transform.tag = "StructureBrick";
	}
}