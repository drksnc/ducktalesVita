using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogoCollision : MonoBehaviour {

	bool bColliding = false;
	ActorInput actor_input;
	Rigidbody actor_rb;
	Transform actor_transform;
	float fLastTimeJump;

	void Start()
	{
		fLastTimeJump = Time.time;
		actor_input = GetComponentInParent<ActorInput>();
		actor_transform = GetComponentInParent<Transform> ();
		actor_rb = GetComponentInParent<Rigidbody> ();
	}

	public bool isPogoColliding()
	{
		return gameObject.activeSelf && bColliding;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (Time.time - fLastTimeJump < 0.3f)
			return;

		if (collider.tag == "Enemy" || 
			collider.gameObject.tag == "Climable" || 
			collider.gameObject.layer == LayerMask.NameToLayer("NonUsable")) 
		{
			bColliding = false;
			return;
		}

		bColliding = true;

		//Moved to DestroyableObject.cs
		if (collider.gameObject.tag == "Destroyable") 
		{
			if (!actor_input.GetPogoJumpFlag ())
				return;
			
			collider.gameObject.GetComponent<DestroyableObject> ().DestroyMe ();	
			actor_input.PogoJump (true);
			bColliding = false;
		}

		if (collider.tag == "EnemyWeakPoint") 
		{
			if (!actor_input.GetPogoJumpFlag ())
				return;

			if (actor_input.TakingDamage ())
				return;

			collider.gameObject.GetComponentInParent<DeathManager> ().Die ();
			actor_input.PogoJump (true);
			bColliding = false;
		}

		fLastTimeJump = Time.time;
	}

	void OnTriggerExit(Collider collider)
	{
		bColliding = false;
	}

	void FixedUpdate()
	{
		if (!gameObject.activeSelf)
			bColliding = false;
	}
}
