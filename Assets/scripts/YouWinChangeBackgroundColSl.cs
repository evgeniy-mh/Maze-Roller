using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YouWinChangeBackgroundColSl : MonoBehaviour {

		private Color currentColor;
		private Image image;
		private float colorChangeTimeStep;
		private mainMenuTextColor mmtc;
		// Use this for initialization
		void Start () {
			mmtc = GameObject.Find ("WinTextColored").GetComponent<mainMenuTextColor> ();
			currentColor= mmtc.currentColor;
			colorChangeTimeStep=mmtc.colorChangeTimeStep;

		image = gameObject.GetComponent<Image> ();
			StartCoroutine (ChangeColorSlow());
		}

		IEnumerator ChangeColorSlow(){
			while (true) {
				currentColor= new Color( mmtc.currentColor.r,mmtc.currentColor.g,mmtc.currentColor.b,0.25f);

			    image.color=currentColor;
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
