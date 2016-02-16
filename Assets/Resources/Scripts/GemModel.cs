using UnityEngine;
using System.Collections;

public class GemModel : MonoBehaviour
{
	private int gemType;		
	private float animClock;
	private Gem owner;			// Pointer to the parent object.
	SpriteRenderer sRender;

	public void init(int gemType, Gem owner) {
		this.owner = owner;
		this.gemType = gemType;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,0);		// Center the model on the parent.
		name = "Gem Model";
		animClock = 0.0f;

		DestroyImmediate(GetComponent<MeshRenderer>());
		DestroyImmediate(GetComponent<MeshCollider>());
		DestroyImmediate(GetComponent<MeshFilter>());
		sRender = gameObject.AddComponent<SpriteRenderer>();
		sRender.sprite = Resources.Load<Sprite>("Textures/gem" + gemType);
	}

	public void animate() {
		animClock += Time.deltaTime;
		transform.localPosition = new Vector3(0,Mathf.Sin(5*animClock)/5,0);
	}

	public void setAlpha(float a) {
		sRender.color = new Color(sRender.color.r, sRender.color.g, sRender.color.b, a);
	}
}

