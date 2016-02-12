using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	GameObject tileFolder;
	GameObject marbleFolder;
	Tile[,] board;
	List<Marble> marbles;
	public bool isRunning = false;
    GemManager gemMan;
	float timeSinceLastGem;
	int score;

    public const int boardWidth = 7;
	public const int boardHeight = 7;
	public const int numTrains = 3;
	public const float TileZ = 0;
	public const float LightZ = -.01f;
	public const float TrainZ = -.02f;
    public const float GemZ = (LightZ + TrainZ) / 2;

	public const int DIRECTION_EAST = 0;
	public const int DIRECTION_NORTH = 1;
	public const int DIRECTION_WEST = 2;
	public const int DIRECTION_SOUTH = 3;

	// Use this for initialization
	void Start() {
		score = 0;
		timeSinceLastGem = 0.0f;
		marbles = new List<Marble>();
		tileFolder = new GameObject();
		tileFolder.name = "Tiles";
		marbleFolder = new GameObject();
		marbleFolder.name = "Marbles";
		board = new Tile [boardWidth, boardHeight];
		List<int> openColumns = new List<int>();
		for (int i = 0; i < 3; i++) {
			addMarble();
		}
		for (int i = 0; i < boardWidth; i++) {
			openColumns.Add(i);
		}
		for (int i = 0; i < boardWidth; i++) {
			for (int j = 0; j < boardHeight; j++) {
				board[i, j] = addTile(i - boardWidth / 2, j - boardHeight / 2);
				int rand = Random.Range(0, 5);
				if (rand == 0) {
					board[i, j].addTurn(Random.Range(0, 4));
				}
			}
		}
		for (int row = 0; row < boardHeight; row++) {
			int colIndex = Random.Range(0, openColumns.Count);
			int col = openColumns[colIndex];
			board[row, col].addTurn(Random.Range(0, 4));
			openColumns.RemoveAt(colIndex);
		}
        GameObject gemManagerObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        gemMan = gemManagerObject.AddComponent<GemManager>();
        gemMan.init(this, board);
		foreach (Marble t in marbles) {
			print("Initializaing a train.\n");
            t.init(this);
        }
	}

	void startMovement() {
		isRunning = true;
	}

	void stopMovement() {
		isRunning = false;
	}
	
	// Update is called once per frame
	void Update() {
		if (!isRunning) {
			return;
		}
		timeSinceLastGem += Time.deltaTime;
		if (gemMan.shouldSpawnGem(timeSinceLastGem)) {
			gemMan.addGem();
			timeSinceLastGem = 0.0f;
		}
		foreach (Marble tm in marbles) {
			tm.updatePosition();
		}
		int mb = -1;
		if (Input.GetMouseButtonDown(0)) {
			mb = 0;
		}
		else if (Input.GetMouseButtonDown(1)) {
			mb = 1;
		}
		if (mb != -1) {
			Vector3 worldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = Mathf.RoundToInt(worldCoords.x) + boardWidth / 2;
			int y = Mathf.RoundToInt(worldCoords.y) + boardHeight / 2;
			bool isTrainOnTile = false;
			foreach (Marble tm in marbles) {
				if (tm.currX == x && tm.currY == y) {
					isTrainOnTile = true;
					break;
				}
			}
			if (!isTrainOnTile && x >= 0 && x < boardWidth && y >= 0 && y < boardHeight) {
				Tile t = board[x, y];
				t.rotateTurn(1 - mb); // left click = clockwise, right click = counterclockwise
			}
		}
	}

	void OnGUI() {
		if (isRunning) {
			if (GUI.Button(new Rect(10, 10, 70, 40), "Pause")) {
				stopMovement();
			}
		}
		else {
			if (GUI.Button(new Rect(10, 10, 70, 40), "Start")) {
				startMovement();
			}
		}
		GUI.Label(new Rect(Screen.width - 100, 0, Screen.width, 50), "Score: " + score);
	}

	public static string getDirectionName(int dir) {
		switch (dir) {
			case 0:
				return "East";
			case 1:
				return "North";
			case 2:
				return "West";
			case 3:
				return "South";
			default:
				return "No direction";
		}
	}

	Tile addTile(float x, float y) {
		GameObject obj = new GameObject();
		obj.name = "Tile";
		Tile t = obj.AddComponent<Tile>();
		t.init(0, this);
		t.transform.parent = tileFolder.transform;
		t.transform.position = new Vector3(x, y, TileZ);
		t.gameObject.layer = LayerMask.NameToLayer("Board");
		return t;
	}

	void addMarble() {
		GameObject marbleObj = new GameObject();
		marbleObj.transform.parent = marbleFolder.transform;
		marbleObj.name = "Marble Container";
		Marble marble = marbleObj.AddComponent<Marble>();
		marbles.Add(marble);
	}

	public void destroyMarble(Marble m) {
		Destroy(m.gameObject);
		marbles.Remove(m);
	}

	public Tile getTile(int x, int y) {
		return board[x, y];
	}

	public Vector3 getTileCoordinates(int x, int y) {
		return board[x, y].transform.position;
	}

	public void onGemPickup() {
		score++;
		print("Score is now: " + score);
	}
}
