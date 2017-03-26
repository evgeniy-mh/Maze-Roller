using UnityEngine;
using System.Collections;

public class levelManager : MonoBehaviour {

	public string level;

    public bool isMultiplayer = false;

	// Use this for initialization
	void Start () {
		level = "small"; //default level
		DontDestroyOnLoad (transform.gameObject);
	}

	public void SetLevel(string level){
		this.level = level;
		//Debug.Log (this.level);
	}
	public string GetLevel(){

		return level;

	}

	public intVector2 GetLevelSize(){
		switch (level) {
		case "small":
			return new intVector2(10,10);

			//break;
		case "normal":
			return new intVector2(15,15);
			//break;
		case "big":

			return new intVector2(20,20);
			///break;
		default:
			Debug.Log ("levelname error");
			return new intVector2(0,0);
			//break;

		}
	}
}
