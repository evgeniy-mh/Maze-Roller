using UnityEngine;
using System.Collections;

public class Cube2Rotator : MonoBehaviour {
	
	public float tumble;

	void FixedUpdate(){
		transform.Rotate (Vector3.up,tumble*Time.deltaTime);
	}

}
