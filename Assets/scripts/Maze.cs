using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	///public
	//public intVector2 size;
	public MazeCell cellPrefab;
	public float generationStepWait;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;
	public GameObject[] BonusPrefabs;
	public GameObject lightPrefab;
	public int SpotLightInterval;
	public Queue<GameObject> tempObjects;
	public int BonusAppearanceChance;
	public int CubeAppearanceChance;
	public int Cube2AppearanceChance;
	public int Cube3AppearanceChance;
	public int cubeCount=0;

	//private
	private intVector2 size;

	private MazeCell[,] cells;
	private ColorWorker cw;
	private MazeCell startCell;
	private int lightCounter;
	private GameObject bonus;
	private GameObject bonusHolder;
	private int[] Corners;
	private bool bonusFlag;
	public Queue<GameObject> wallsQueue;


////////////////////////////////////////////////////////

	public MazeCell GetCell(intVector2 coordinates){
		return cells[coordinates.x, coordinates.z];
	}

	public void Generate (intVector2 mazeSize) {
		size = mazeSize;

		Corners = new int[] {0,size.x - 1,size.z - 1};
		tempObjects = new Queue<GameObject> ();
		wallsQueue= new Queue<GameObject> ();
		cw = gameObject.GetComponent<ColorWorker> ();
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell> ();

		bonusHolder = GameObject.Find ("bonusHolder");

		DoFirstGenerationStep (activeCells);
		while (activeCells.Count > 0) {
			DoNextGenerationStep (activeCells);
		}
		//после того как лабиринт сгенерировался он должен быть прозрачным;
		//PaintMaze (Color.gray,0.5f);
	}

	public void PaintMaze(Color color, float alpha){
		StartCoroutine( cw.changeColorSlow (tempObjects,tempObjects.ToArray().Length,color,alpha));
		StartCoroutine (DestroyPaintStack());
	}
	
	IEnumerator DestroyPaintStack(){
		while (true) {
			if (cw.IsPaintingDone ()) {
				//Debug.Log ("Painting done");
				tempObjects.Clear ();
				yield return 0;
				break;
			}
			yield return new WaitForSeconds (1f);
		}
	}	

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		startCell = CreateCell (GetRandomCorner ());
		activeCells.Add(startCell);
		lightCounter = 0;
	}
	
	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		intVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction,false);////////////////////////////////////////////
				// No longer remove the cell here.
			}
		}
		else {
			CreateWall(currentCell, null, direction,true);
			// No longer remove the cell here.
		}
	}

	private MazeCell CreateCell (intVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition =
			new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);

		tempObjects.Enqueue (newCell.gameObject);
		RandomBonus (newCell.transform);
		//CreateSpotLights (newCell.transform);

		return newCell;
	}

	private void CreateSpotLights(Transform cell){
		if (lightCounter == SpotLightInterval)
			lightCounter = 0;
		else lightCounter++;
		
		if (lightCounter == 0) {
			PutSpotLight(cell);
		}
	}

	private void PutSpotLight(Transform currentLightTransform){
		GameObject light = Instantiate (lightPrefab);
		light.transform.position=new Vector3(currentLightTransform.position.x+0.25f,currentLightTransform.position.y+2f,currentLightTransform.position.z+0.25f);
		light.transform.parent = currentLightTransform;
	}	

	private void RandomBonus(Transform currentCellTransform){
		
		if (Random.Range (0, BonusAppearanceChance) == BonusAppearanceChance-1) {
		    bonusFlag = false;
			

			if((Random.Range(0,CubeAppearanceChance)==CubeAppearanceChance-1) && !bonusFlag){
				CreateRandomBonus(BonusPrefabs[0],currentCellTransform,bonusHolder);
				cubeCount++;
				bonusFlag = true;				
			}

			if((Random.Range(0,Cube2AppearanceChance)==Cube2AppearanceChance-1) && !bonusFlag){
				CreateRandomBonus(BonusPrefabs[1],currentCellTransform,bonusHolder);
				bonusFlag = true;
			}

			if((Random.Range(0,Cube3AppearanceChance)==Cube3AppearanceChance-1) && !bonusFlag){
				CreateRandomBonus(BonusPrefabs[2],currentCellTransform,bonusHolder);
				bonusFlag = true;
			}
		}
	}

	private void CreateRandomBonus(GameObject bonus, Transform bonusTransform, GameObject parent){
		Vector3 position=new Vector3(bonusTransform.position.x,bonusTransform.position.y+0.5f,bonusTransform.position.z);

		bonus=Instantiate(bonus);
		bonus.transform.position=position;
		bonus.transform.parent = parent.transform;

		//Debug.Log ("CreateRandomBonus at"+position.ToString()+"  "+parent.transform.ToString());

	}

	public intVector2 GetRandomCorner(){
		return new intVector2(Corners[Random.Range(0,Corners.Length)],Corners[Random.Range(0,Corners.Length)]);
	}
	
	public bool ContainsCoordinates(intVector2 coordinate){
		return
			coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;

	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}
	
	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction, bool isCornerWall) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);

		if (!isCornerWall) {
			wallsQueue.Enqueue (wall.gameObject);
		}

		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());

			//if (!isCornerWall) {
			wallsQueue.Enqueue(wall.gameObject);
			//}
		}
	}

	public void SetWallsVisibility(bool visibility,int time){  
		foreach (GameObject wall in wallsQueue) {
			wall.GetComponentInChildren<BoxCollider>().enabled=visibility;
		}
		StartCoroutine (CheckWallsVisibility(true,time));
	}

	public void SetWallsVisibility(bool visibility){

		foreach (GameObject wall in wallsQueue) {
			wall.GetComponentInChildren<BoxCollider>().enabled=visibility;
		}
	}

	private IEnumerator CheckWallsVisibility(bool visibility,int deltaTime){//visibility - состояние в которое должна вернутся стена
		for(int i=0; i<=deltaTime;i++){

			//Debug.Log ("CheckWallsVisibility"+i);
			yield return new WaitForSeconds (1);
		}
		SetWallsVisibility (visibility);

	}

	public void Update(){

	}
	

	public Vector3 GetStartCellPosition(){
		return startCell.gameObject.transform.position;
	}
}





















