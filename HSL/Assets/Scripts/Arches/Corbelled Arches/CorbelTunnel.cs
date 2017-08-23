using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//gets attached to a corbel object in the Hierarchy
public class CorbelTunnel : MonoBehaviour {

	public void Tunnel () {
		//create an empty game object in the Hierarchy that will contain all the small pieces, aka parent object
		GameObject parent = new GameObject ("Tunnel");
		//getting dimensions of the arch from the Corbel object in Hierarchy 
		float brickDepth = this.gameObject.GetComponent<Corbel>().brickDepth;
		float vaultDepth = this.gameObject.GetComponent<Corbel>().vaultDepth;
		float brickHeight = this.gameObject.GetComponent<Corbel>().brickHeight;
		float overhang = this.gameObject.GetComponent<Corbel>().overhang;
		//taking the starting position of the arch into account, eg yPos can be = 3 meters above the ground, etc.
		float height = brickHeight + this.gameObject.GetComponent<Corbel> ().yPos;
		//make the tunnel a slightly wider than just the gaps between corbel bricks, to make sure that when boolean
		//difference is performed, small pieces of meshes are not left between the tunnel and the arch/vault itself.
		//Thus, add 0.0001 factor to make tunnel overlap with the structure a little bit.
		float length = this.gameObject.GetComponent<Corbel>().freeSpan + 0.0001f;

		float depth;
		//this function that comes with System.Diagnostics lets you know what method is calling the current method
		StackTrace stackTrace = new StackTrace();
		//if the calling method is Corbelled Arch, then make depth = brickdepth
		if (stackTrace.GetFrame (1).GetMethod ().Name == "CorbelledArch") {
			depth = brickDepth;
		}
		//if the calling method is Corbelled Vault (only other method that calls CorbelTunnel), then make depth = vault depth
		else {
			depth = vaultDepth;
		}
	
		//get how many rectangular bricks are needed to make the tunnel 
		int count = this.gameObject.GetComponent<Corbel> ().count;

		for (int t = 0; t < count; t++) {
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			//assign a tag so that each brick can be referenced
			cube.transform.tag = "Tunnel";
			//assign the parent object as the parent for every brick
			cube.transform.parent = parent.transform;
			//for the first brick, use the initial values, for next ones use the modified values
			cube.transform.localScale = new Vector3 (length, height, depth);
			//y position is half the height to make it appear above the "ground", this piece might need some modification later
			cube.transform.position = new Vector3 (0, height/2, 0);
			//decrease the length of next bricks by twice the overhang (because decreases by an overhang value from each side)
			length -= 2 * overhang;
			//height increases by a brick height each time
			height += brickHeight;
		}
	}
}
