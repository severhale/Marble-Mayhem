using UnityEngine;
using System.Collections;

public class Elephant : MonoBehaviour {
	public float age = 0.0f;
	bool warmingUp = true;
	float warmupTime;
	float bottomBoardEdgeY;
	float topBoardEdgeY;

	public const float lifeSpan = 20.0f;

	public void init(Vector3 position, float warmupTime) {
		transform.position = position;
		this.warmupTime = warmupTime;
		warmingUp = true;
		bottomBoardEdgeY = -BoardManager.boardHeight / 2;
		topBoardEdgeY = -bottomBoardEdgeY;
	}

	public void updatePosition() {
		float deltaTime = Time.deltaTime;
		age += deltaTime;
		if (warmingUp) {
			if (age >= warmupTime) {
				warmingUp = false;
			}
		}
		else {
			if (transform.position.y <= bottomBoardEdgeY) {
				transform.position = new Vector3(transform.position.x, topBoardEdgeY + 1 - (bottomBoardEdgeY - transform.position.y), transform.position.z);
			}
			else {
				transform.Translate(new Vector3(0, -2 * Time.deltaTime, 0));
			}
		}
	}

	public bool shouldDieOfOldAge() {
		return age >= lifeSpan;
	}
}