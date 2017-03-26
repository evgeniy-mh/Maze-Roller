using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class gameController : MonoBehaviour {
	
	public GameObject playerPrefab;
	private PlayerController myPlayer;
    private GameObject player;
	private GameObject winPanel;

	public CameraController CameraPrefab;
	public levelManager lm;

	private void Start () {
		winPanel = GameObject.Find ("WinPanel");
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			RestartGame();
		}

		if (myPlayer.transform.position.y < -10f)
			RestartGame ();

	}

	/*public AudioSource audioSource;
	public GameObject audioSourcePrefab;*/
	public Maze mazePrefab;
	private Maze mazeInstance;
	private bonusHolder currentBonusHolder;
	private CameraController myCamera;
	private GameObject PauseMenuButton;



	private void BeginGame () {
		currentBonusHolder = GameObject.Find ("bonusHolder").GetComponent<bonusHolder>();

		mazeInstance = Instantiate(mazePrefab) as Maze;


		lm = GameObject.Find ("levelManager").GetComponent<levelManager>();
		mazeInstance.Generate (lm.GetLevelSize());


        //myPlayer=GameObject.Find("Player").GetComponent<PlayerController>();

        if(!lm.isMultiplayer)   player = Instantiate(playerPrefab);
        else
        {
            player = GameObject.Find("Player(Clone)");
        }

        myPlayer=player.GetComponent<PlayerController>();
        myPlayer.InitializePlayer ();

		myCamera= Instantiate (CameraPrefab);

		myCamera.SetPosition (new Vector3(0,100,-100));
		myCamera.SetPlayer (myPlayer.gameObject);


		myPlayer.SetNewLocation (mazeInstance.GetStartCellPosition());

		//покраска сеттинга
		ColorScheme csch = new ColorScheme ();
		Color32[] colorScheme = csch.getColorRandomScheme ();
		mazeInstance.PaintMaze (colorScheme[0],0.5f);
		myPlayer.PaintPlayer (colorScheme[1]);

	    /*byte cameraBackgroudColorDelta = 30;
		Color cameraBackgroudColor = new Color ((float)(colorScheme[0].r-cameraBackgroudColorDelta),(float)(colorScheme[0].g-cameraBackgroudColorDelta),(float)(colorScheme[0].b-cameraBackgroudColorDelta));
		Debug.Log ("gameController: "+colorScheme[0].ToString());
		Debug.Log ("cameraBackgroudColor= "+cameraBackgroudColor);
		myCamera.setBackgroudColor (cameraBackgroudColor);*/
		Color color = colorScheme [0];
		//Debug.Log (color);
		color = new Color (color.r-0.25f,color.g-0.25f,color.b-0.25f,1f);
		myCamera.setBackgroudColor (color);

		//currentBonusHolder.PaintBonuses (colorScheme[2]);
		StartCoroutine (currentBonusHolder.PaintBonusesSlow (colorScheme[2]));


		winPanel.SetActive (false);
		StartCoroutine (CheckPlayerWin());

		PauseMenuButton = GameObject.Find ("MenuButton(onPause)");
		PauseMenuButton.SetActive (false);
	}

	IEnumerator CheckPlayerWin(){
		while (true) {
			if (myPlayer.playerWin) {
				winPanel.SetActive (true);
				//StartCoroutine (SlowlyShowWinPanel());
				break;
			}
			//Debug.Log ( CheckPlayerWin());
			yield return new WaitForSeconds (0.5f);
		}
	}

	IEnumerator SlowlyShowWinPanel(){
		/*winPanel.GetComponent<Image> ().color;
		Debug.Log("SlowlyShowWinPanel()");
		/*Vector3 position=winPanel.transform.position;
		Vector3 endPosition = new Vector3 (position.x, -10, position.z);*/
		/*
		RectTransform position=winPanel.GetComponent<RectTransform>();
		RectTransform endPosition = new RectTransform ();
		endPosition.pos = 0;
		endPosition.position.y = -30;*/

		/*while(winPanel.transform.position!=endPosition){
			winPanel.transform.position = new Vector3 (position.x,position.y-1f,position.z);


			yield return new WaitForSeconds (0.1f);
		}*/
		//winPanel.GetComponent<RectTransform>().position = endPosition.position;

		yield return new WaitForSeconds (0.5f);
	}

public void RestartGame () {

		StopAllCoroutines ();
		Destroy(mazeInstance.gameObject);
		currentBonusHolder.Clear ();
		//Destroy (myPlayer.gameObject);
		//myPlayer.SetNewLocation();
		Destroy (myCamera.gameObject);

		Destroy (myPlayer.playerFlare);
	PauseMenuButton.SetActive (true);
	//Destroy (audioSource.gameObject);
		BeginGame();
	}

public void GoToMenu(){
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

	    Destroy (GameObject.Find ("levelManager"));
        Destroy(GameObject.Find("Audio Source"));
        Destroy(GameObject.Find("MultiplayerManager"));
        SceneManager.LoadScene ("mainMenu");

}

public void PauseGame(){
		
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		PauseMenuButton.SetActive (false);
		} else {
			Time.timeScale = 0;
		PauseMenuButton.SetActive (true);
	}

}
	

}
