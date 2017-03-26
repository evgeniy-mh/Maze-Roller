using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mainMenuTextColor : MonoBehaviour {
	public float deltaHue;
	public Color currentColor;
	public float colorChangeTimeStep;

	private Text text;
	private float hue;

	// Use this for initialization
	void Start () {
		text=gameObject.GetComponent<Text>();
		hue = 0f;
		//text.color = Color.Lerp (Color.cyan,Color.red,0.1f);
		StartCoroutine (ChangeHueSlow());
	}

	IEnumerator ChangeHueSlow(){
		while (true) {
			if(hue<1){
				currentColor=Color.HSVToRGB(hue,0.8f,0.8f);
				text.color=currentColor;
				hue+=deltaHue;
			}
			else hue=0;

			//Debug.Log(text.color);
			yield return new WaitForSeconds(colorChangeTimeStep);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
