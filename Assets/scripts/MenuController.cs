using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	private Color32[] colors;

	private GameObject mainElements;
	private GameObject AboutElements;

	void Start(){
		ColorScheme cs= new ColorScheme();
		colors=cs.getColorRandomScheme();

		mainElements = GameObject.Find ("mainElements");
		AboutElements = GameObject.Find ("AboutElements");

		AboutElements.SetActive (false);

		GameObject.Find ("Plane").GetComponent<Renderer>().material.color=colors[0];

		Color color = colors [0];
		color= new Color (color.r-0.25f,color.g-0.25f,color.b-0.25f,1f);
		GameObject.Find ("Main Camera").GetComponent<Camera> ().backgroundColor = color;
	}

	public void StartGame(){
		SceneManager.LoadScene ("main");

	}

	public void ExitGame(){
		Application.Quit ();

	}

	public void About(){
		
		mainElements.SetActive (false);
		AboutElements.SetActive (true);
	}

	public void AboutBack(){

		mainElements.SetActive (true);
		AboutElements.SetActive (false);
	}

	public void MyButton(){
		Application.OpenURL ("https://vk.com/evgeniy_mh");

	}

}
