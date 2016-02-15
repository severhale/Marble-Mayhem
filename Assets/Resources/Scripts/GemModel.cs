using UnityEngine;
using System.Collections;

public class GemModel : MonoBehaviour
{
	private int gemType;		
	private float clock;		// Keep track of time since creation for animation.
	private Gem owner;			// Pointer to the parent object.
	SpriteRenderer sRender;

	public void init(int gemType, Gem owner) {
		this.owner = owner;
		this.gemType = gemType;

		transform.parent = owner.transform;					// Set the model's parent to the gem.
		transform.localPosition = new Vector3(0,0,0);		// Center the model on the parent.
		name = "Gem Model";									// Name the object.
		clock = 0.0f;

		DestroyImmediate(GetComponent<MeshRenderer>());
		DestroyImmediate(GetComponent<MeshCollider>());
		DestroyImmediate(GetComponent<MeshFilter>());
		sRender = gameObject.AddComponent<SpriteRenderer>();
		sRender.sprite = Resources.Load<Sprite>("Textures/gem" + gemType);
//		mat = GetComponent<Renderer>().material;								// Get the material component of this quad object.
//		mat.mainTexture = Resources.Load<Texture2D>("Textures/gem" + gemType);	// Set the texture.  Must be in Resources folder.
//		mat.color = new Color(1,1,1);											// Set the color (easy way to tint things).
//		mat.shader = Shader.Find ("Sprites/Default");						// Tell the renderer that our textures have transparency. 
	}

	public void updateOnFrame () {
		
		// Incrememnt the clock based on how much time has elapsed since the previous update.
		// Using deltaTime is critical for animation and movement, since the time between each call
		// to Update is unpredictable.
		clock += Time.deltaTime;

		// A more serious project would probably use subclasses for gem types, or at least enums instead of 
		// this silly gemType int plus a bunch of if statements.  But it'll do for here.  

		if (gemType == 1) { // set rotation around z for spin effect
			transform.eulerAngles = new Vector3(0,0,360*clock);
		}
		if (gemType == 2) { // set position y for bounce effect
			transform.localPosition = new Vector3(0,Mathf.Sin(5*clock)/5,0);	
		}	
		if (gemType == 3) { // set scale in x and y for pulse effect
			transform.localScale = new Vector3(1+Mathf.Sin(3*clock)/2,1+Mathf.Sin(3*clock)/2,1);
		}
		if (gemType == 4) { // set color for twinkle effect
			sRender.color = new Color(1+0.5f*Mathf.Sin(8*clock),1+0.5f*Mathf.Sin(8*clock),1+0.5f*Mathf.Sin(8*clock));
		}
	}

	public void setAlpha(float a) {
		sRender.color = new Color(sRender.color.r, sRender.color.g, sRender.color.b, a);
	}
}

