using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollision : MonoBehaviour {

	private uint ground_collision_counter = 0;

	public bool isCollidingGround()
	{
		return ground_collision_counter > 0;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			ground_collision_counter++;
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			ground_collision_counter--;
		}
	}
}
