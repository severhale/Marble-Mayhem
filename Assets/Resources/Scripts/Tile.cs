using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public const int TILE_BLANK = 0;
	public const int TILE_PIT = 1;
	public const int TILE_WALL = 2;
	public const int TURN_LIGHTS = 3;


    private int type = TILE_BLANK;
	private int turnCount;
	private BoardManager board;
	private GameObject tileObj, turnObj;
	private TileModel tileMod;
	private TurnLightsModel turnMod;

	public void init(int tileType, BoardManager board) {
		this.board = board;
		turnCount = 0;
		tileObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
		tileMod = tileObj.AddComponent<TileModel>();
		tileMod.init(this, type);
	}

	public void addTurn(int rotation) {
		if (turnCount == 0) {
			turnObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
			turnObj.layer = LayerMask.NameToLayer("Lights");
			turnMod = turnObj.AddComponent<TurnLightsModel>();
			turnMod.init(this, rotation);
			turnCount++;
		}
//		else if (turnCount == 1) {
//			turnObj2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
//			turnObj2.layer = LayerMask.NameToLayer("Lights");
//			turnMod2 = turnObj2.AddComponent<TurnLightsModel>();
//			turnMod2.init(this, rotation);
//			turnMod2.setActive(!turnMod.isActive());
//			turnCount++;
//		}
		else {
			print("No more than one turn signal per tile.");
		}
	}

	// Rotate turn clockwise or counterclockwise
	public void rotateTurn(int clockwise) {
		if (turnMod != null) {
			if (clockwise == 1) {
				turnMod.rotateClockwise();
			}
			else {
				turnMod.rotateCounterClockerwise();
			}
		}
//		if (turnMod2 != null) {
//			turnMod2.rotate90Degrees();
//		}
	}

	public int numTurnSignals() {
		return turnCount;
	}

	//if entering from the <inputDirection> (east, west, etc.), which way will you go after passing through this tile?
	public int getOutputDirection(int enteringFrom) {
		if (turnCount == 1) {
			return turnMod.isActive() ? turnMod.getOutputDirection(enteringFrom) : getOppositeDirection(enteringFrom);
//			case 2:
//				int dir = TurnLightsModel.getSharedDirection(turnMod, turnMod2);
//				if (dir != -1 && // if they overlap
//				    	(enteringFrom == turnMod.getOutputDirection(dir) ||
//						enteringFrom == turnMod2.getOutputDirection(dir))) { // and the input leads to the overlapping direction
//					return dir; // return overlapping direction
//				}
//				else {
//					if (turnMod.isActive()) {
//						return turnMod.getOutputDirection(enteringFrom);
//					}
//					else {
//						return turnMod2.getOutputDirection(enteringFrom);
//					}
//				}
		}
		else {
			return getOppositeDirection(enteringFrom);
		}
	}

	public static int getOppositeDirection(int direction) {
		return (direction + 2) % 4;
	}

	public static Vector3 getRelativeExitPoint(int exitDirection) {
		switch (exitDirection) {
			case BoardManager.DIRECTION_EAST:
				return Vector3.right;
			case BoardManager.DIRECTION_NORTH:
				return Vector3.up;
			case BoardManager.DIRECTION_WEST:
				return Vector3.left;
			case BoardManager.DIRECTION_SOUTH:
				return Vector3.down;
			default:
				return Vector3.right;
		}
	}

	public Vector3 getEntryPoint(int enteringFrom) {
		switch (enteringFrom) {
			case BoardManager.DIRECTION_EAST:
				return transform.position + .5f * Vector3.right;
			case BoardManager.DIRECTION_NORTH:
				return transform.position + .5f * Vector3.up;
			case BoardManager.DIRECTION_WEST:
				return transform.position + .5f * Vector3.left;
			case BoardManager.DIRECTION_SOUTH:
				return transform.position + .5f * Vector3.down;
			default:
				return transform.position + .5f * Vector3.right;
		}
	}
}

