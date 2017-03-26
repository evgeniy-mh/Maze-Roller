using UnityEngine;
using System.Collections;

public class bonusHolder : MonoBehaviour {

	private GameObject[] bonuses;
	public byte deltaColor;



	public void Clear(){
		//Debug.Log ("Clear");
		bonuses = GameObject.FindGameObjectsWithTag ("bonusObjectHolder");
		foreach(GameObject obj in bonuses){
			//Debug.Log(obj);
			Destroy(obj.gameObject);
		}
	}

	public void PaintBonuses(Color32 color){
		Debug.Log ("PaintBonuses");
		bonuses = GameObject.FindGameObjectsWithTag ("bonusObject");

		foreach (GameObject obj in bonuses) {
			if (obj != null) {
				try {
					
					obj.GetComponent<Renderer> ().material.color =color;

				} catch (MissingReferenceException e) {
					Debug.Log (e);
				}
			}
		}
	}

	public IEnumerator PaintBonusesSlow(Color32 color){
		yield return new WaitForSeconds (1f);
		bonuses = GameObject.FindGameObjectsWithTag ("bonusObject");
		
		foreach (GameObject obj in bonuses) {
			if (obj != null) {
				try {


					obj.GetComponent<Renderer> ().material.color = color;



				} catch (MissingReferenceException e) {
					Debug.Log (e);
				}
				//Debug.Log("painting bonuses)))");

				//Debug.Log(obj.ToString());

				yield return new WaitForSeconds (0.2f);
			}
		}

	}

}
