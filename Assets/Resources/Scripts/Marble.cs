using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour {
	public int direction;
	public float relativeSpeed;
	public int currX, currY; // integer tile positions;
	public int health;
//	float clock; // time since entered tile;
	float percentComplete;
	BoardManager board;
	MarbleModel model;
	GameObject marbleObj;
    private bool initialized = false;

	private bool boosted = false;
	private bool boostCooldown = false;
	private float timeSinceBoostStart = 0.0f;
	private float timeSinceBoostEnd = 0.0f;

	private float boostTime = 1.0f;
	private float boostCooldownTime = 5.0f;

	private float tempTimeBuffer = 0.0f;

	// Use this for initialization
	public void init(BoardManager bm) {
        board = bm;
		if (board == null) {
			print("Error. board is null.");
		}
		marbleObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = marbleObj.AddComponent<MarbleModel>();
		model.init(this);

		// Init the public values
		currX = Random.Range(0, BoardManager.boardWidth);
		currY = Random.Range(0, BoardManager.boardHeight);
		health = 100;
		direction = Random.Range(0, 4);
		relativeSpeed = 1.0f;

		transform.eulerAngles = new Vector3(0, 0, (direction + 3) * 90);
		int enteringFrom = Tile.getOppositeDirection(direction);
		transform.position = board.getTile(currX, currY).getEntryPoint(enteringFrom);
		transform.position = new Vector3(transform.position.x, transform.position.y, BoardManager.TrainZ);
//		clock = 0f;
        initialized = true;
	}
	
	// Update is called once per frame
	public void updatePosition() {
		if (!checkHealth() || !initialized) {
			return;
		}

		if (boosted) {
			duringBoost();
		}
		else if (boostCooldown) {
			duringBoostCooldown();
		}

		Tile currTile = board.getTile(currX, currY);
		Vector3 entryPoint, exitPoint, exitPointRel;
		int enteringFrom = Tile.getOppositeDirection(direction);
		int outputDir = currTile.getOutputDirection(enteringFrom);

		bool done = false;

		if (outputDir == enteringFrom) {
			direction = Tile.getOppositeDirection(enteringFrom);
			percentComplete = 0f;
//			clock = 0f;
			return;
		}

//		clock += Time.deltaTime;
		percentComplete += Time.deltaTime * relativeSpeed;
		if (percentComplete >= 1) {
			done = true;
//			clock = 0f;
			percentComplete = 0f;
		}
		entryPoint = currTile.getEntryPoint(enteringFrom);
		exitPointRel = Tile.getRelativeExitPoint(outputDir);
		exitPoint = currTile.transform.position + .5f * exitPointRel;

		Debug.DrawLine(entryPoint, exitPoint);

		if (outputDir == Tile.getOppositeDirection(enteringFrom)) {
			if (done) {
				currX += (int) exitPointRel.x;
				currY += (int) exitPointRel.y;
			}
			else {
				transform.position = Vector3.Lerp(entryPoint, exitPoint, percentComplete);
                transform.position = new Vector3(transform.position.x, transform.position.y, BoardManager.TrainZ);
            }
		}
		else { // damn, it's a curve.
			if (done) {
				transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Rad2Deg * Mathf.Atan2(exitPointRel.y, exitPointRel.x));
				currX += (int) exitPointRel.x;
				currY += (int) exitPointRel.y;
				direction = outputDir;
			}
			else {
				Vector3 pivot = entryPoint + .5f * exitPointRel;
				int rotationDirection = 1;
				if (outputDir == (enteringFrom + 1) % 4) {
					rotationDirection = -1;
				}
				transform.RotateAround(pivot, rotationDirection * Vector3.forward, Time.deltaTime * 90 * relativeSpeed);
                transform.position = new Vector3(transform.position.x, transform.position.y, BoardManager.TrainZ);
                transform.rotation.Set(0, 0, transform.rotation.z, transform.rotation.w);
			}
		}
		if (done) {
			currX = (BoardManager.boardWidth + currX) % BoardManager.boardWidth;
			currY = (BoardManager.boardHeight + currY) % BoardManager.boardWidth;
			transform.position = board.getTileCoordinates(currX, currY) - .5f * exitPointRel;
            transform.position = new Vector3(transform.position.x, transform.position.y, BoardManager.TrainZ);
//			print("Done. New tile: " + currX + ", " + currY);
//			print("Position: " + transform.position);
		}
	}

	public void handleMouseClick() {
		if (board.isRunning && !boostCooldown && !boosted) {
			boosted = true;
			boostCooldown = false;
			timeSinceBoostStart = 0.0f;
			relativeSpeed = 2.0f;
		}
	}

	void duringBoost() {
		timeSinceBoostStart += Time.deltaTime;
		if (timeSinceBoostStart >= boostTime) {
			boosted = false;
			relativeSpeed = 1.0f;
			timeSinceBoostEnd = 0.0f;
			boostCooldown = true;
		}
	}

	void duringBoostCooldown() {
		timeSinceBoostEnd += Time.deltaTime;
		if (timeSinceBoostEnd >= boostCooldownTime) {
			boostCooldown = false;
		}
	}

	public void onCollisionWithMarble() {
		print("Shoot.");
		health -= 50;
		checkHealth();
	}

	public bool checkHealth() {
		if (health <= 0) {
			board.destroyMarble(this);
			return false;
		}
		return true;
	}
}