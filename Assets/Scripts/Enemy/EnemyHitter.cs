using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitter : MonoBehaviour {

	Rigidbody actor_rigid;
	ActorInput actor_input;
	BeagleEnemy parent_behaviour;
	public int damage = 1;

	// Use this for initialization
	void Start () {
		actor_rigid = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ();
		actor_input = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorInput> ();
		parent_behaviour = GetComponentInParent<BeagleEnemy> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider)
	{
		GameObject who = collider.gameObject;
		if (who.tag == "Player") {

			Debug.Log (gameObject.tag);

			if (!GetComponentInParent<DeathManager> ().Alive ())
				return;

			if (actor_input.TakingDamage ())
				return;

			actor_rigid.velocity = Vector3.zero;
			actor_rigid.angularVelocity = Vector3.zero;
			actor_rigid.angularDrag = 0;

			if (parent_behaviour.FacingRight())
				actor_rigid.AddForce ((Vector3.right + Vector3.up) * 5.0f, ForceMode.Impulse);
			else
				actor_rigid.AddForce ((Vector3.left + Vector3.up) * 5.0f, ForceMode.Impulse);


			collider.gameObject.GetComponent<DamageControl> ().ReceiveDamage (damage, gameObject);
		}
	}
}
