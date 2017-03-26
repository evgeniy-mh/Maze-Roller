using UnityEngine;
using System.Collections;

public class ColorWorker: MonoBehaviour  {
	
	private bool isPaintingDone;
	private Component[] temp;

	private void Start () {
		isPaintingDone = false;
	}

	public IEnumerator changeColorSlow(IEnumerable stack, int stackLength, Color color, float alpha){
		/*Debug.Log ("changeColorSlow");
		Debug.Log ("stackLength "+stackLength);
		foreach (GameObject element  in stack) {

			Debug.Log(element.name);

		}*/

		foreach (GameObject element  in stack) {
				temp = element.GetComponentsInChildren<Renderer> ();
			
				foreach (Renderer rend in temp) {

					rend.material.color = new Color (color.r, color.g, color.b, alpha);
				}

				stackLength--;

				if (stackLength == 0) {
				//Debug.Log("isPaintingDone = true");
					isPaintingDone = true;
				break;
				}
				yield return new WaitForSeconds (0.01f);
			
		}
	}

	public bool IsPaintingDone(){
		return isPaintingDone;
	}


}
