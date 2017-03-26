using UnityEngine;
using System.Collections;


[System.Serializable]

public struct intVector2 {

	public int x, z;
	public intVector2(int x, int z){
		this.x = x;
		this.z = z;
	}

	public static intVector2 operator + (intVector2 a, intVector2 b) {
		a.x += b.x;
		a.z += b.z;
		return a;
	}
}