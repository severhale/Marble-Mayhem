using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElephantManager : MonoBehaviour {
	List<Elephant> elephants;
	BoardManager bm;

	public void init(BoardManager bm) {
		this.bm = bm;
		elephants = new List<Elephant>();
	}

	public void updateOnFrame() {
		for (int i = elephants.Count - 1; i >= 0; i--) {
			if (elephants[i].shouldDieOfOldAge()) {
				Elephant e = elephants[i];
				elephants.RemoveAt(i);
				Destroy(e.gameObject);
			}
			else {
				elephants[i].updatePosition();
			}
		}
	}

	public void addElephant() {
		Vector3 position = new Vector3(Random.Range(-BoardManager.boardWidth / 2, BoardManager.boardWidth / 2 + 1), BoardManager.boardHeight / 2 + 1.5f, -.04f);
		GameObject elephantObj = (GameObject)Resources.Load("Prefabs/Elephant Container", typeof(GameObject));
		Elephant newElephant = Instantiate(elephantObj).GetComponent<Elephant>();
		print("Adding new elephant at: " + position);
		elephants.Add(newElephant);
		newElephant.init(position, 5.0f);
	}
}

