using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeagleEnemy : MonoBehaviour {

	Rigidbody enemy_rigid;

	public SpriteRenderer sprite;
	public float MovingSpeed = 2.0f;

	float fLastTimeSwapDir = 0.0f;

	bool bFacingRight = true;
	// Use this for initialization
	void Start () {
		enemy_rigid = GetComponent<Rigidbody> ();
		Physics.IgnoreCollision (GameObject.FindGameObjectWithTag ("Player").GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		UpdateWalking ();
	}
		
	void OnCollisionEnter(Collision coll_info)
	{
		GameObject who = coll_info.gameObject;
		if (who.tag == "Player") {
			Physics.IgnoreCollision (coll_info.collider, GetComponent<BoxCollider>());
		} else {

			if (Time.time - fLastTimeSwapDir < 0.1f)
				return;

			fLastTimeSwapDir = Time.time;
			FlipSprite ();
		}	
	}
		
	void FlipSprite()
	{
		bFacingRight = !bFacingRight;
		sprite.flipX = !bFacingRight;
	}

	void UpdateWalking()
	{
		Vector3 res_vel = enemy_rigid.velocity;
		res_vel.x = 0;

		if (bFacingRight) {
			res_vel.x = MovingSpeed;
		} else {
			res_vel.x = -MovingSpeed;
		}

		enemy_rigid.velocity = res_vel;
	}

	public bool FacingRight()
	{
		return bFacingRight;
	}
}
