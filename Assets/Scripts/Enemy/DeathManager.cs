using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour {

	Rigidbody rb;
	public SpriteRenderer shadow;
	bool bAlive = true;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!bAlive && !GetComponentInChildren<Renderer> ().isVisible)
			gameObject.SetActive(false);
	}

	public bool Alive()
	{
		return bAlive;
	}

	public void Die()
	{
		if (!Alive ())
			return;

		shadow.enabled = false;
		bAlive = false;
		gameObject.layer = LayerMask.NameToLayer ("UnitPhantom");

		GetComponent<Animator> ().SetBool ("Alive", false);

		rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
		rb.constraints &= ~RigidbodyConstraints.FreezePositionY;

		rb.AddForce ((Vector3.up + Vector3.back) * 5.0f, ForceMode.Impulse);

	}
}
