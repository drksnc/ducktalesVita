using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

	// Use this for initialization
	public string dir = "ToRight";
	bool force_dir = false;
	GameObject actor;

	float hitStartTime = 0.0f;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		actor = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - hitStartTime > 0.5f)
			rb.constraints |= RigidbodyConstraints.FreezePositionX;
	}

	public void OnHit()
	{
		rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
		hitStartTime = Time.time;

		if (!force_dir) 
		{
			if (actor.GetComponent<ActorInput> ().bFacingRight)
				rb.AddForce (Vector3.right * 20.0f, ForceMode.Impulse);
			else
				rb.AddForce (Vector3.left * 20.0f, ForceMode.Impulse);
		} 
		else 
		{
			if (dir == "ToRight") {
				rb.AddForce (Vector3.right * 20.0f, ForceMode.Impulse);
			}
		}
	}
}
