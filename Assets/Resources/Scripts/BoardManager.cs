using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	GameObject tileFolder;
	GameObject trainFolder;
	Tile[,] board;
	List<TrainMovement> trains;
	bool isRunning = false;
//    GemManager gemMan;

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
		trains = new List<TrainMovement>();
		tileFolder = new GameObject();
		tileFolder.name = "Tiles";
		trainFolder = new GameObject();
		trainFolder.name = "Marbles";
		board = new Tile [boardWidth, boardHeight];
		List<int> openColumns = new List<int>();
		for (int i = 0; i < 3; i++) {
			addTrain();
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
//        GameObject gemManagerObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
//        gemMan = gemManagerObject.AddComponent<GemManager>();
//        gemMan.init(this, board);
		foreach (TrainMovement t in trains) {
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
		if (isRunning) {
			foreach (TrainMovement tm in trains) {
				tm.updatePosition();
			}
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
			foreach (TrainMovement tm in trains) {
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

	void addTrain() {
		GameObject train = GameObject.CreatePrimitive(PrimitiveType.Quad);
		train.transform.parent = trainFolder.transform;
		Material mat = train.GetComponent<Renderer>().material;
		mat.mainTexture = Resources.Load<Texture2D>("Textures/marble");
		mat.color = new Color(1, 1, 1);
		mat.shader = Shader.Find("Transparent/Diffuse");
		train.name = "Marble";
		train.tag = "Player";
		TrainMovement movement = train.AddComponent<TrainMovement>();
		trains.Add(movement);
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

	public Tile getTile(int x, int y) {
		return board[x, y];
	}

	public Vector3 getTileCoordinates(int x, int y) {
		return board[x, y].transform.position;
	}
}
