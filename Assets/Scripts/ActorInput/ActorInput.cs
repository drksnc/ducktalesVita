using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
× button KeyCode.JoystickButton0
○ button KeyCode.JoystickButton1
□ button KeyCode.JoystickButton2
△ button KeyCode.JoystickButton3
L button KeyCode.JoystickButton4
R button KeyCode.JoystickButton5
SELECT button KeyCode.JoystickButton6
START button KeyCode.JoystickButton7
up button KeyCode.JoystickButton8
right button KeyCode.JoystickButton9
down button KeyCode.JoystickButton10
left button KeyCode.JoystickButton11
*/

public class ActorInput : MonoBehaviour {

	public GameObject actor;
	public SpriteRenderer sprite;
	public BoxCollider ground_check_coll;

	public float MovingSpeed = 5.0f;
	public float JumpForce = 7.5f;
	public float fPogoJumpBonus = 2.0f;
	public LayerMask GroundMask;
	public GameObject PauseMenu;

	public delegate void CurrentCutscene();

	public GroundCollision gr_coll;
	public PogoCollision pogo_coll;

	float fFallingThreshold = -0.4f;
	float fJumpingSpeedDt = 2.0f;

	public bool bFacingRight = true;

	bool bInputEnabled = true;

	bool bJumping = false;
	bool bOnGround = true;
	bool bWasOnGround = true;
	bool bMoving = false;
	bool bDucking = false;
	bool bPogoJump = false;
	bool bCollisionAhead = false;
	bool bCanHitCollidibleObject = false;
	bool bIntersectsWithClimableObject = false;
	bool bClimbing = false;

	bool bMoveRightManually = false;
	bool bMoveLeftManually = false;
	bool bCanFlipSprite = true;

	float CollisionAheadStartTime = 0.0f;
	GameObject object_we_can_hit = null;
	public BoxCollider collider_we_climbing = null;

	bool bNeedRewindIdleAnimation = false;


	[Header("Sounds")]
	public AudioSource SoundPogo;
	public AudioSource SoundLand;
	public AudioSource SoundCantHit;
	public AudioSource SoundClimb;

	Rigidbody actor_rigid;
	Animator animator;
	DamageControl damage_controller;

	void OnCollisionEnter(Collision collision_info)
	{
	
	}

	void OnCollisionStay(Collision collision_info)
	{
		bCollisionAhead = false;
		bCanHitCollidibleObject = false;

		if (collision_info.gameObject.layer == LayerMask.NameToLayer("Ground")) 
		{
			if (bFacingRight) {
				if (Mathf.Approximately (collision_info.contacts [0].normal.x, -1.0f))
				{
					bCollisionAhead = true;
					bCanHitCollidibleObject = collision_info.gameObject.tag == "Movable";
					CollisionAheadStartTime = Time.time;
					object_we_can_hit = collision_info.gameObject;
				}
			} 
			else 
			{
				if (Mathf.Approximately (collision_info.contacts [0].normal.x, 1.0f))
				{
					bCollisionAhead = true;
					bCanHitCollidibleObject = collision_info.gameObject.tag == "Movable";
					CollisionAheadStartTime = Time.time;
					object_we_can_hit = collision_info.gameObject;
				}
			}
		}
			
	}
		
	// Use this for initialization
	void Start () {
		actor_rigid = actor.GetComponent<Rigidbody> ();
		animator = actor.GetComponent<Animator> ();
		damage_controller = actor.GetComponent<DamageControl> ();
	}

	public void PauseGame(bool v)
	{
		if (v) {
			Time.timeScale = 0.0f;
			PauseMenu.SetActive (true);
		} else {
			Time.timeScale = 1.0f;
			PauseMenu.SetActive(false);
		}
	}

	void Update()
	{
		UpdateControls ();
		UpdateClimbing ();

		animator.SetBool ("Ducking", bDucking);
	}
		
	// Update is called once per frame
	void FixedUpdate()
	{
		CheckOnGround ();
		CheckCollisionAhead ();
		FixedUpdateControls ();

		animator.SetFloat ("Speed", MovingSpeed);
		animator.SetBool ("IsOnGround", bOnGround);
		animator.SetBool ("Moving", bMoving);
		animator.SetBool ("Falling", IsFalling ());
		animator.SetBool ("Jumping", bJumping);
		animator.SetBool ("CollidingWithObstacle", bCollisionAhead);
		animator.SetBool ("CanHitObject", bCanHitCollidibleObject);
		animator.SetBool ("Climbing", bClimbing);

		if (!bClimbing) 
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("landing")) 
			{
				actor_rigid.constraints |= RigidbodyConstraints.FreezePositionX;
			} 
			else 
			{
				actor_rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
			}
		}
	}

	void UpdateControls()
	{

		if (Input.GetKeyDown (KeyCode.JoystickButton7) || Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (!PauseMenu.activeSelf) 
			{
				PauseGame (true);
			} 
			else 
			{
				PauseGame (false);
			}
		}

		if (!bInputEnabled)
			return;
		
		if (Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown (KeyCode.X)) { // Press Cross
			Jump ();
		} 

		if (Input.GetKeyDown (KeyCode.JoystickButton1) || 
			Input.GetKeyUp (KeyCode.JoystickButton2)   ||
			Input.GetKeyDown (KeyCode.C)) { // Press Circle

			if (!bCollisionAhead)
				SetPogoJumpFlag (true);
			else
				TryHitObject ();
		}

		if (Input.GetKeyUp (KeyCode.JoystickButton0) || Input.GetKeyUp (KeyCode.X)) { //Release Cross
			Fall ();
		} 

		if (Input.GetKeyUp (KeyCode.JoystickButton1) || 
			Input.GetKeyUp (KeyCode.JoystickButton2) || 
			Input.GetKeyUp (KeyCode.C)) { //Release Circle
			SetPogoJumpFlag(false);
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton10) || Input.GetKeyDown(KeyCode.DownArrow)) //Press Down
		{
			if (!bClimbing)
				bDucking = true;
		}

		if (Input.GetKeyUp (KeyCode.JoystickButton10) || Input.GetKeyUp (KeyCode.DownArrow)) //Release Down
		{
			if (!bClimbing) 
			{
				bDucking = false;
				animator.SetBool ("DuckingIdle", false);
			}
		}
	}

	void FixedUpdateControls()
	{
		bMoving = false;
		Vector3 res_velocity = actor_rigid.velocity;

		if (bClimbing)
			res_velocity.y = 0;
		
		if (!TakingDamage())
			res_velocity.x = 0;

		float moving_speed = MovingSpeed;

		if (bJumping)
			moving_speed -= fJumpingSpeedDt;

		if (bInputEnabled && (Input.GetKey (KeyCode.JoystickButton11) || Input.GetKey (KeyCode.LeftArrow))
			|| (!bInputEnabled && bMoveLeftManually)) { // Left

				if (!bDucking && !TakingDamage ()) {
					res_velocity.x = -moving_speed;

					if (bFacingRight)
						FlipSprite ();

					bMoving = true;
					DisableLandingAnim ();
				}

		} else if (bInputEnabled && (Input.GetKey (KeyCode.JoystickButton9) || Input.GetKey (KeyCode.RightArrow))
			|| !bInputEnabled && bMoveRightManually) { //right

				if (!bDucking && !TakingDamage ()) {
					res_velocity.x = moving_speed;
					//actor_rigid.velocity = moving_speed * Vector3.right;

					if (!bFacingRight)
						FlipSprite ();

					bMoving = true;
					DisableLandingAnim ();	
				}
			} 

			animator.SetBool ("ClimbingUp", false);
			animator.SetBool ("ClimbingDown", false);

			if (Input.GetKey (KeyCode.JoystickButton8) || Input.GetKey (KeyCode.UpArrow)) { //Up
				if (bIntersectsWithClimableObject) {
					bClimbing = true;
					res_velocity.y = moving_speed;

					animator.SetBool ("ClimbingUp", true);
				}
			} else if (Input.GetKey (KeyCode.JoystickButton10) || Input.GetKey (KeyCode.DownArrow)) { //Down
				if (bIntersectsWithClimableObject) {
					bClimbing = true;
					res_velocity.y = -moving_speed;

					animator.SetBool ("ClimbingDown", true);
				}
			}
	
		actor_rigid.velocity = res_velocity;
	}

	void ResetClimbing()
	{
		bClimbing = false;
		actor_rigid.constraints &= ~RigidbodyConstraints.FreezePositionX;
		//actor_rigid.constraints &= ~RigidbodyConstraints.FreezePositionY;
		actor_rigid.useGravity = true;
	}

	void UpdateClimbing()
	{
		if (collider_we_climbing == null)
		{
			return;
		}

		if (collider_we_climbing.bounds.Intersects (GetComponent<BoxCollider> ().bounds)) 
		{
			bIntersectsWithClimableObject = true;
		} 
		else 
		{
			bIntersectsWithClimableObject = false;

			if (bClimbing)
				ResetClimbing ();

			return;
		}
		
		if (!bClimbing)
			return;
						
		if (TakingDamage () || bJumping || IsFalling()) 
		{
			ResetClimbing ();
			return;
		}

		float offsetX = 0.5f;

		actor.transform.position = new Vector3(collider_we_climbing.transform.position.x + offsetX, actor_rigid.position.y, actor_rigid.position.z);
		actor_rigid.constraints |= RigidbodyConstraints.FreezePositionX;
		//actor_rigid.constraints |= RigidbodyConstraints.FreezePositionY;
		actor_rigid.useGravity = false;
	}

	void TryHitObject()
	{
		if (bJumping)
			return;

		if (IsFalling ())
			return;

		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("hit_prepare"))
			return;

		animator.SetBool ("HittingObject", true);
		bInputEnabled = false;
	}

	void ActualHit()
	{
		if (object_we_can_hit == null)
			return;
		
		if (object_we_can_hit.GetComponent<MovableObject> () == null)
			return;
		
		object_we_can_hit.GetComponent<MovableObject> ().OnHit ();
	}

	void CantHit()
	{
		SoundCantHit.Play ();
	}

	void CheckCollisionAhead()
	{
		if (Time.time - CollisionAheadStartTime > 0.1f)
			bCollisionAhead = false;

		if (!bCollisionAhead)
			CollisionAheadStartTime = 0.0f;
	}

	void SetPogoJumpFlag(bool v)
	{
		if (v) {
			
			if (bOnGround)
				return;

			if (bJumping)
				return;

			if (bClimbing)
				return;
		}

		bPogoJump = v;
		animator.SetBool ("Pogo", bPogoJump);
	}

	public bool GetPogoJumpFlag()
	{
		return bPogoJump;
	}

	public void PogoJump(bool bForce = false)
	{
		if (TakingDamage () && !bForce) {
			SoundPogo.Stop ();
			return;
		}
		//float jump_power = actor_rigid.velocity.magnitude;
		actor_rigid.velocity = Vector3.zero;
		actor_rigid.angularVelocity = Vector3.zero;
		actor_rigid.AddForce (Vector3.up * (JumpForce + fPogoJumpBonus), ForceMode.Impulse);

		//Prevent multiple sounds play
		//if (SoundPogo.time > 0.5f)
			SoundPogo.Play ();
	}

	public void Jump()
	{
		if (!bOnGround) {
			Debug.Log ("Cant Jump: Not on ground!");
			return;
		}
		if (bJumping) {
			Debug.Log ("Cant Jump: Already jumping!");
			return;
		}

		if (TakingDamage())
			return;

		bJumping = true;
		animator.SetBool ("Landing", false);

		actor_rigid.AddForce (Vector3.up * JumpForce, ForceMode.Impulse);
	}

	public void Fall()
	{
		if (IsFalling ()) {
			Debug.Log ("Fall failed: already falling");
			return;
		}

		Debug.Log ("Falling...");

		actor_rigid.AddForce (Vector3.down * 5.0f, ForceMode.VelocityChange);
	}

	void FlipSprite()
	{
		if (bClimbing || !bCanFlipSprite)
			return;
		
		bFacingRight = !bFacingRight;
		sprite.flipX = !bFacingRight;
	}

	public void PlayAnim(string anim_name)
	{
		string anim = "Base Layer." + anim_name;
		animator.Play (anim);
	}

	bool CanGoIdle()
	{
		if (bJumping)
			return false;

		if (!bOnGround)
			return false;

		return true;
	}

	bool IsFalling()
	{
		return actor_rigid.velocity.y < fFallingThreshold;
	}

	public bool TakingDamage()
	{
		return animator.GetBool ("TakingDamage");
	}

	public bool FlashingAfterTakingDamage()
	{
		return damage_controller.Invul ();
	}

	void CheckOnGround()
	{
		if (bPogoJump && pogo_coll.isPogoColliding ()) {
			PogoJump ();
			bWasOnGround = false;
			return;
		}
			
		bOnGround = gr_coll.isCollidingGround ();

		if (bOnGround) {
			if (bJumping) {
				bJumping = false;

				if (GetPogoJumpFlag ())
					SetPogoJumpFlag (false);
			}

			actor_rigid.useGravity = false;

			if (!bWasOnGround) {
				bWasOnGround = true;
				OnLanding ();
			}
		} else {
			bWasOnGround = false;

			if (!bClimbing)
				actor_rigid.useGravity = true;
		}
	}

	void OnLanding()
	{
		EnableLandingAnim ();
		animator.SetBool ("TakingDamage", false);

		if (!SoundLand.isPlaying && Time.timeSinceLevelLoad > 0.1f)
			SoundLand.Play ();
	}

	void OnLandingAnimFinished()
	{
		DisableLandingAnim ();
	}

	void DisableLandingAnim()
	{
		animator.SetBool ("Landing", false);
	}

	void EnableLandingAnim()
	{
		if (Time.timeSinceLevelLoad < 0.1f)
			return;
		
		StartCoroutine (LandingAnimCoroutine ());
	}

	IEnumerator LandingAnimCoroutine()
	{
		yield return new WaitForSeconds (0.05f);

		if (!bMoving && !bDucking) {
			PlayAnim ("landing");
			animator.SetBool ("Landing", true);
		}
	}

	void OnIdleAnimStarted()
	{
		if (!bNeedRewindIdleAnimation)
			return;
		
		animator.SetFloat ("AnimSpeed", 1.0f);
	}

	void OnIdleAnimFinished()
	{
		animator.SetFloat ("AnimSpeed", -1.0f);
		bNeedRewindIdleAnimation = true;
	}

	void OnDuckStartAnimFinished()
	{
		//PlayAnim ("duck_idle");
		animator.SetBool ("DuckingIdle", true);
	}

	void OnHitAnimationEnd()
	{
		animator.SetBool ("HittingObject", false);
		bInputEnabled = true;
	}

	void OnStrictInAnimationEnd()
	{
		animator.SetBool ("CutSceneStrict_idle", true);
	}

	void PlayClimbSnd()
	{
		if (!SoundClimb.isPlaying)
			SoundClimb.Play ();
	}

	public void DisableInput()
	{
		SetPogoJumpFlag (false);
		bInputEnabled = false;
	}

	public void EnableInput()
	{
		bInputEnabled = true;
	}

	public void MoveRight()
	{
		bMoveRightManually = true;
	}

	public void MoveLeft()
	{
		bMoveLeftManually = true;
	}

	public void StopManualMove()
	{
		bMoveRightManually = false;
		bMoveLeftManually = false;
	}

	public bool IsOnGround()
	{
		return bOnGround;
	}

	public void CanFlipSprite(bool v)
	{
		bCanFlipSprite = v;
	}
}
