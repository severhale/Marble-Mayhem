using UnityEngine;
using System.Collections;

public class TrainMovement : MonoBehaviour {
	public int direction;
	public float relativeSpeed = 1.0f;
	public int currX, currY; // integer tile positions;
	float clock; // time since entered tile;
	BoardManager board;
    private bool initialized = false;

	// Use this for initialization
	public void init(BoardManager bm) {
		direction = Random.Range(0, 4);
        board = bm;
		if (board == null) {
			print("Error. board is null.");
		}
		currX = Random.Range(0, BoardManager.boardWidth);
		currY = Random.Range(0, BoardManager.boardHeight);

		transform.eulerAngles = new Vector3(0, 0, (direction + 3) * 90);
		int enteringFrom = Tile.getOppositeDirection(direction);
		transform.position = board.getTile(currX, currY).getEntryPoint(enteringFrom);
		transform.position = new Vector3(transform.position.x, transform.position.y, BoardManager.TrainZ);
//		switch (direction) {
//			case BoardManager.DIRECTION_EAST:
//				transform.Translate(0.5f, 0, 0);
//				break;
//			case BoardManager.DIRECTION_NORTH:
//				transform.Translate(0, 0.5f, 0);
//				break;
//			case BoardManager.DIRECTION_SOUTH:
//				transform.Translate(0, -0.5f, 0);
//				break;
//			case BoardManager.DIRECTION_WEST:
//				transform.Translate(-0.5f, 0, 0);
//				break;
//		}
		clock = 0f;
        initialized = true;
	}
	
	// Update is called once per frame
	public void updatePosition() {
        if (!initialized) {
            return;
        }
		Tile currTile = board.getTile(currX, currY);
		Vector3 entryPoint, exitPoint, exitPointRel;
		int enteringFrom = Tile.getOppositeDirection(direction);
		int outputDir = currTile.getOutputDirection(enteringFrom);

		bool done = false;

		if (outputDir == enteringFrom) {
			direction = Tile.getOppositeDirection(enteringFrom);
			clock = 0f;
			return;
		}

		clock += Time.deltaTime;
		if (clock * relativeSpeed >= 1) {
			done = true;
			clock = 0f;
		}
		entryPoint = currTile.getEntryPoint(enteringFrom);
		exitPointRel = Tile.getRelativeExitPoint(outputDir);
		exitPoint = currTile.transform.position + .5f * exitPointRel;

		Debug.DrawLine(entryPoint, exitPoint);

		if (outputDir == Tile.getOppositeDirection(enteringFrom)) {
			if (done) {
				currX += (int) exitPointRel.x;
				currY += (int) exitPointRel.y;
				clock = 0f;
			}
			else {
				transform.position = Vector3.Lerp(entryPoint, exitPoint, clock * relativeSpeed);
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
			print("Done. New tile: " + currX + ", " + currY);
			print("Position: " + transform.position);
		}
	}
}