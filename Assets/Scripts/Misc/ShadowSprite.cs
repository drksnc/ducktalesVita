using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour {

	Transform parent_transform;
	public SpriteRenderer sprite;
	public float y_offset = 1.0f;
	int[] layers_to_ignore = { 0, 0 };

	float previous_dist;
	float max_radius = 0.8f;
	float min_radius = 0.3f;

	// Use this for initialization
	void Start () {
		parent_transform = GetComponentInParent<Transform> ();
		layers_to_ignore [0] = LayerMask.NameToLayer ("Unit");
		layers_to_ignore [1] = LayerMask.NameToLayer ("UnitPhantom");
	}

	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Vector3 pos = parent_transform.position;
		pos.y -= y_offset;

		Physics.Raycast (pos, Vector3.down, out hit);

		if (!hit.collider || !hit.collider.gameObject)
			return;

		if (hit.collider.gameObject.layer == layers_to_ignore[0] || 
			hit.collider.gameObject.layer == layers_to_ignore[1])
			return;

		pos = hit.point;
		pos.y += 0.1f;
		sprite.transform.position = pos;
		float dist = Vector3.Distance (parent_transform.position, pos);

		if (dist < 1.0f)
			dist = max_radius;
		else if (dist < min_radius)
			dist = min_radius;

		if (!Mathf.Approximately(previous_dist, dist))
		{
			Vector3 scale = Vector3.one;

			if (dist > 1.0f)
				scale *= 1 / dist;
			else
				scale *= dist;

			previous_dist = dist;
			sprite.transform.localScale = scale;
		}
	}
}
