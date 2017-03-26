using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour {

	public GameObject playerFlarePrefab;
	public Material playerTransparent;
	public int speed;
	public int jumpForce;
	public int WallsVisibilityDeltaTime;
	public bool playerWin;

	private SimpleTouchPad stp;
	private int playerScore;
	private Text ScoreText;
	private Text BonusText;
	private Text BonusText1;
	private bool canJump;
	private bool invisibleWalls;
	private Maze mazeInstance;
	private Vector2 direction;

	private float moveHorizontal;
	private float moveVertical;
	private Vector3 movement;
	private Rigidbody myRb;
	public GameObject playerFlare;
	private Material playerMaterial;
	private int cubeCount;

////////////////////////////////////////////////

	void Start(){
		/*canJump = false;
		invisibleWalls = false;
		myRb = gameObject.GetComponent<Rigidbody> ();
		InitializeText ();
		//setScore (0);

		mazeInstance = GameObject.FindObjectOfType<Maze>();
		cubeCount = mazeInstance.cubeCount;
		ShowCubeCount();
		playerMaterial = gameObject.GetComponent<Renderer> ().material;
//		Debug.Log (playerMaterial);
		stp=GameObject.Find("MovementZone").GetComponent<SimpleTouchPad>();*/
	}	

	public void InitializePlayer(){
		canJump = false;
		invisibleWalls = false;
		myRb = gameObject.GetComponent<Rigidbody> ();
		InitializeText ();
		//setScore (0);

		mazeInstance = GameObject.FindObjectOfType<Maze>();
		cubeCount = mazeInstance.cubeCount;
		ShowCubeCount();
		playerMaterial = gameObject.GetComponent<Renderer> ().material;
		//		Debug.Log (playerMaterial);
		stp=GameObject.Find("MovementZone").GetComponent<SimpleTouchPad>();

	}

	private void InitializeText(){
		ScoreText = GameObject.Find ("ScoreText").GetComponent<Text>();
		BonusText = GameObject.Find ("BonusText").GetComponent<Text>();
		BonusText1 = GameObject.Find ("BonusText1").GetComponent<Text>();
		BonusText.text = "";
		BonusText1.text = "";

	}

	void Update(){
        //if (photonView.isMine)
        //{

            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Jump();

            }
       // }



		/*if (NeedToCheckWallsVisibilityTime) {
			if ((int)Time.time > WallsVisibilityStartTime + WallsVisibilityDeltaTime) {

			}
			else{
				BonusText1.text="Invisible walls for"+;
			}
			//Debug.Log("NeedToCheckWallsVisibilityTime");
		}*/

	}
	void FixedUpdate(){
       // if (photonView.isMine)
            InputMovement();
    } 

    void InputMovement()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (myRb != null)
        {

            myRb.AddForce(movement * speed);

            direction = stp.GetDirection();
            movement = new Vector3(direction.x, 0.0f, direction.y);
            myRb.AddForce(movement * speed);
        }

        if (playerFlare != null)
        {

            playerFlare.transform.position = transform.position;

        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
            stream.SendNext(myRb.position);
        else
            myRb.position = (Vector3)stream.ReceiveNext();
    }

    public void Jump(){
		//Debug.Log("jump");
		if (canJump) {
			myRb.AddForce (new Vector3 (0, 1, 0) * jumpForce);
			canJump=false;
			BonusText.text="";
		}
		
	}

	 

	void OnTriggerEnter(Collider myTrigger){
		switch(myTrigger.gameObject.name){
		case "cube(Clone)": 
			Destroy (myTrigger.gameObject);
			//addScore(50);
			cubeCount--;
			ShowCubeCount ();

			if (cubeCount == 0) {
				playerWin = true;
				//PlayerWin ();
			}

			break;
		case "cube2(Clone)":
			if(!canJump){
			Destroy(myTrigger.gameObject);
			canJump=true;
			BonusText.text="You can jump now";
			
			}
			break;
		case "cube3(Clone)": 
			if(!invisibleWalls){
			Destroy(myTrigger.gameObject);
			mazeInstance.SetWallsVisibility(false,WallsVisibilityDeltaTime);
			StartCoroutine( CheckWallsVisibility(WallsVisibilityDeltaTime));
			invisibleWalls=true;
			
			
				StartCoroutine( SlowlyMakePlayerInvisible(WallsVisibilityDeltaTime,0.1f,0.2f));
			}
			
			//BonusText1.text="Invisible walls for";
			break;
		}
	}

	private IEnumerator CheckWallsVisibility(int deltaTime){//visibility - состояние в которое должна вернутся стена
		for(int i=0; i<=deltaTime;i++){
			BonusText1.text="Invisible walls for "+(deltaTime-i)+" sec";
			yield return new WaitForSeconds (1);
		}
		BonusText1.text="";
		invisibleWalls = false;
		
	}
		

	public void SetNewLocation(Vector3 position){
		this.transform.position= new Vector3(position.x,position.y+5,position.z);
		myRb.velocity = Vector3.zero;
		//Debug.Log (this.transform.position);
	}
		
	public void ShowCubeCount(){
		changeScoreText (cubeCount+" cubes left");

	}

	private void changeScoreText(string text){
		ScoreText.text = text;
	}


	public void PaintPlayer(Color32 color){
		playerMaterial = gameObject.GetComponent<Renderer> ().material;
		playerMaterial.color = color;
	}

	public IEnumerator SlowlyMakePlayerInvisible(float onTime,float deltaTime, float minAlpha){
		//Debug.Log(gameObject.GetComponent<Renderer> ().material.color);
		Color color = playerMaterial.color;
		int steps = 0;

		gameObject.GetComponent<Renderer> ().material = playerTransparent;
		gameObject.GetComponent<Renderer> ().material.color = playerMaterial.color;
		playerFlare = Instantiate (playerFlarePrefab);


		while(color.a>minAlpha){

			playerMaterial.color=new Color(color.r,color.g,color.b,color.a-0.05f);
			color = playerMaterial.color;

			//Debug.Log(gameObject.GetComponent<Renderer> ().material.color);
			steps++;
			yield return new WaitForSeconds(deltaTime);
		}
		yield return new WaitForSeconds(onTime-deltaTime*steps*2);

		while(color.a<1){
			
			playerMaterial.color=new Color(color.r,color.g,color.b,color.a+0.05f);
			color = playerMaterial.color;
			
			//Debug.Log(gameObject.GetComponent<Renderer> ().material.color);
			steps++;
			yield return new WaitForSeconds(deltaTime);
		}
		gameObject.GetComponent<Renderer> ().material = playerMaterial;
		Destroy (playerFlare);

	}

}
