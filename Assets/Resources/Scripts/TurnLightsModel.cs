using UnityEngine;
using System.Collections;

public class TurnLightsModel : TileModel {
	private bool active;
	private int direction1, direction2;

	public override void init(Tile parent, int rotation) {
		base.init(parent, Tile.TURN_LIGHTS);
		setActive(true);
		direction1 = (BoardManager.DIRECTION_SOUTH + rotation) % 4;
		direction2 = (BoardManager.DIRECTION_EAST + rotation) % 4;
		transform.localPosition = new Vector3(0, 0, BoardManager.LightZ);
		transform.eulerAngles = new Vector3(0, 0, rotation * 90);
	}

	// rotates 90 degrees counter-clockwise
	public void rotateCounterClockerwise() {
		transform.Rotate(0, 0, 90);
		direction1 = (direction1 + 1) % 4;
		direction2 = (direction2 + 1) % 4;
	}

	public void rotateClockwise() {
		transform.Rotate(0, 0, -90);
		direction1 = (direction1 + 3) % 4;
		direction2 = (direction2 + 3) % 4;
	}

	public void toggleActive() {
		setActive(!active);
	}

	public void setActive(bool flag) {
		active = flag;
		if (active) {
			GetComponent<Renderer>().material.color = new Color(1.5f, 1.5f, 1.5f, 1);
		}
		else {
			GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.4f);
		}
	}

	public int getOutputDirection(int enteringFrom) {
		if (enteringFrom == direction1) {
			return direction2;
		}
		else if (enteringFrom == direction2) {
			return direction1;
		}
		else {
			return Tile.getOppositeDirection(enteringFrom);
		}
	}

	public bool isActive() {
		return active;
	}

	// return the direction the two curves share, or -1 if they don't share one or overlap
	public static int getSharedDirection(TurnLightsModel m1, TurnLightsModel m2) {
		if (m1.direction1 == m2.direction2) {
			return m1.direction1;
		}
		else if (m1.direction2 == m2.direction1) {
			return m1.direction2;
		}
		else {
			return -1;
		}
	}
}

