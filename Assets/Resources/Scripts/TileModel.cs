using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour {
	protected Tile parent;
	protected int type;
	protected Material mat;

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
		mat = GetComponent<Renderer>().material;
		switch (type) {
			case Tile.TILE_BLANK:
				mat.mainTexture = Resources.Load<Texture2D>("Textures/tileBlank");
				break;
			case Tile.TILE_PIT:
				mat.mainTexture = Resources.Load<Texture2D>("Textures/tilePit");
				break;
			case Tile.TILE_WALL:
				mat.mainTexture = Resources.Load<Texture2D>("Textures/tileWall");
				break;
			case Tile.TURN_LIGHTS:
				mat.mainTexture = Resources.Load<Texture2D>("Textures/turnLights");
				break;
			default:
				mat.mainTexture = Resources.Load<Texture2D>("Textures/tileBlank");
				break;
		}
		mat.color = new Color(1, 1, 1);
		mat.shader = Shader.Find("Transparent/Diffuse");
	}
}

