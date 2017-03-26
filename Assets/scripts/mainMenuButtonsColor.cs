using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mainMenuButtonsColor : MonoBehaviour {
	public string mainObjName;

	private Color currentColor;
	private Button button;
	private float colorChangeTimeStep;
	private mainMenuTextColor mmtc;


	// Use this for initialization
	void Start () {
		Initialize ();
	}

	void OnEnable(){
		Initialize ();

	}

	private void Initialize(){
		mmtc = GameObject.Find (mainObjName).GetComponent<mainMenuTextColor> ();
		currentColor= mmtc.currentColor;
		colorChangeTimeStep=mmtc.colorChangeTimeStep;

		button = gameObject.GetComponent<Button> ();
		StartCoroutine (ChangeColorSlow());
	}

	IEnumerator ChangeColorSlow(){
		while (true) {
			currentColor= new Color( mmtc.currentColor.r,mmtc.currentColor.g,mmtc.currentColor.b,0.5f);

			ColorBlock cb=button.colors;
			cb.normalColor=currentColor;
			cb.highlightedColor=new Color( mmtc.currentColor.r+0.3f,mmtc.currentColor.g+0.3f,mmtc.currentColor.b+0.3f,0.5f);
			cb.pressedColor=new Color( mmtc.currentColor.r-0.5f,mmtc.currentColor.g-0.5f,mmtc.currentColor.b-0.5f,0.5f);;
			button.colors=cb;
			yield return new WaitForSeconds(colorChangeTimeStep);
		}
	}

	void Update(){

	}
	
	/*IEnumerator ChangeHueSlow(){
		while (true) {
			if(hue<1){
				button.colors.normalColor =Color.HSVToRGB(hue,0.8f,0.8f);
				hue+=deltaHue;
			}
			else hue=0;
			
			//Debug.Log(text.color);
			yield return new WaitForSeconds(0.1f);
		}
	}*/
}
