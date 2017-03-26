using UnityEngine;
using System.Collections;

public class audioSourceScript : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}
}
