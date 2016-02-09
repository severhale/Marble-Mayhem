using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour {
	protected Tile parent;
	protected int type;

	public virtual void init(Tile parent, int type) {
		this.parent = parent;
		this.type = type;

		if (type == Tile.TURN_LIGHTS) {
			name = "Turn Model";
		}
		else {
			name = "Tile Model";
		}

		transform.parent = parent.transform;
		transform.localPosition = new Vector3(0, 0, BoardManager.TileZ);
		switch (type) {
			case Tile.TILE_BLANK:
				GetComponent<Renderer>().material = (Material)Resources.Load("Textures/tileBlankMaterial", typeof(Material));
				break;
			case Tile.TILE_PIT:
				GetComponent<Renderer>().material = (Material)Resources.Load("Textures/tileBlankMaterial", typeof(Material));
				break;
			case Tile.TILE_WALL:
				GetComponent<Renderer>().material = (Material)Resources.Load("Textures/turnLightsMaterial", typeof(Material));
				break;
			case Tile.TURN_LIGHTS:
				GetComponent<Renderer>().material = (Material)Resources.Load("Textures/turnLightsMaterial", typeof(Material));
				break;
			default:
				GetComponent<Renderer>().material = (Material)Resources.Load("Textures/tileBlankMaterial", typeof(Material));
				break;
		}
	}
}

