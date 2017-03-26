using UnityEngine;
using System.Collections;

public class RandomCreator : MonoBehaviour {
	public GameObject[] bonusPrefabs;
	public Material playerTransparent;
	public float deltaCreationTime;


	private ColorScheme cs;
	private Color32[] colors;
	// Use this for initialization
	void Start () {
		cs= new ColorScheme();
		colors=cs.getColorRandomScheme();
		StartCoroutine (work());

	}

	IEnumerator work(){
		GameObject newObject;
		while (true) {

			newObject=Instantiate(bonusPrefabs[Random.Range(0,bonusPrefabs.Length)]);
			newObject.transform.position=GetRandomCoordinates();
			PaintBonus (newObject,colors[2]);
			StartCoroutine (SlowlyDestroy(newObject,3f,0.01f));

			colors=cs.getColorRandomScheme();
			yield return new WaitForSeconds (deltaCreationTime);
		}
	}

	public void PaintBonus(GameObject bonus,Color32 color){
		if (bonus != null) {

			bonus.GetComponentInChildren<Renderer> ().material.color =color;

			}
		}


	public IEnumerator SlowlyDestroy(GameObject obj,float onTime,float deltaTime){
		
		Material objMaterial = obj.GetComponentInChildren<Renderer> ().material;
		Color color = objMaterial.color;

		//Debug.Log (obj.GetComponentInChildren<Renderer> ().material);

		obj.GetComponentInChildren<Renderer> ().material = playerTransparent;
		obj.GetComponentInChildren<Renderer> ().material.color = objMaterial.color;

		yield return new WaitForSeconds (onTime);//задержка перед удалением
		while(color.a>0){

			obj.GetComponentInChildren<Renderer> ().material.color=new Color(color.r,color.g,color.b,color.a-0.01f);
			color = obj.GetComponentInChildren<Renderer> ().material.color;

			//Debug.Log(obj.GetComponentInChildren<Renderer> ().material.color);
			yield return new WaitForSeconds(deltaTime);
		}
		Destroy (obj);


	}

	private Vector3 GetRandomCoordinates(){
		return new Vector3(Random.Range(-8f,8f),1,Random.Range(-4f,4f));


	}
}
