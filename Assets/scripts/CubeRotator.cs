using UnityEngine;
using System.Collections;

public class CubeRotator : MonoBehaviour {
	
	private Rigidbody myRb;
	public float tumble;

	void Start () {
		myRb = GetComponent<Rigidbody> ();

		myRb.angularVelocity = Random.insideUnitSphere * tumble; 
	}

}
