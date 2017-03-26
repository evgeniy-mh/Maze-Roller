using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour {

    public GameObject playerPrefab;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(transform.gameObject);
        PhotonNetwork.ConnectUsingSettings("1.0");
	}
	
	public void ConnectToRoom(){
		if (PhotonNetwork.countOfRooms == 0) {
            Debug.Log("Creating new room...");
            RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 5 };
            PhotonNetwork.CreateRoom ("test",roomOptions,TypedLobby.Default);

            GameObject.Find("MenuController").GetComponent<MenuController>().StartGame();
        } else {
            Debug.Log("Connecting to existing room...");

            //GameObject.Find("levelManager").GetComponent<levelManager>().isMultiplayer = true;
            PhotonNetwork.JoinRoom("test");
            
        }	

	}


    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        SceneManager.LoadScene("main");
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
    }
}
