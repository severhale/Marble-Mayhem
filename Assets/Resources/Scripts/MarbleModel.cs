using UnityEngine;
using System.Collections;

public class MarbleModel : MonoBehaviour {
	Marble marble;
	// Use this for initialization
	public void init(Marble parent) {
		this.marble = parent;
		transform.parent = parent.transform;
		transform.localPosition = new Vector3(0, 0, 0);
		Renderer renderer = GetComponent<Renderer>();
		renderer.material = (Material)Resources.Load("Textures/TrainMaterial", typeof(Material));
		name = "Marble Model";
		tag = "Player";
		DestroyImmediate(gameObject.GetComponent<Collider>());
		CircleCollider2D coll = gameObject.AddComponent<CircleCollider2D>();
		coll.isTrigger = true;
		coll.radius = 0.35f;
		Rigidbody2D rigidBody = gameObject.AddComponent<Rigidbody2D>();
		rigidBody.isKinematic = true;
		rigidBody.gravityScale = 0;
	}

	public void OnMouseDown() {
		marble.handleMouseClick();
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			marble.onCollisionWithMarble();
		}
		else if (other.gameObject.tag == "Enemy") {
			marble.onCollisionWithEnemy();
		}
	}

	public void setColor(Color c) {
		GetComponent<Renderer>().material.color = c;
	}
}

