using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageControl : MonoBehaviour {

	bool bInvul = false;
	public float InvulSec = 2.0f;
	public int Lives = 6;
	float InvulStartTime = 0.0f;

	Rigidbody actor_rigid;
	Animator animator;
	public SpriteRenderer actor_sprite;
	public GameObject LivesUI;

	public AudioSource SoundHit1;
	public AudioSource SoundHit2;
	public AudioSource SoundHit3;

	AudioSource[] SoundsHit = new AudioSource[3];

	// Use this for initialization
	void Start () 
	{
		actor_rigid = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();

		SoundsHit [0] = SoundHit1;
		SoundsHit [1] = SoundHit2;
		SoundsHit [2] = SoundHit3;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateInvulLayer ();
	}

	public bool Invul()
	{
		return bInvul;
	}

	public void ReceiveDamage(int damage, GameObject who)
	{
		if (bInvul)
			return;

		Lives -= damage;
		UpdateLivesUI ();

		InvulStartTime = Time.time;
		bInvul = true;

		gameObject.layer = LayerMask.NameToLayer ("PlayerInvul");
		StartCoroutine (UpdateFlashing ());

		animator.SetBool ("TakingDamage", true);

		SoundsHit[Random.Range (0, 3)].Play();
	}

	void UpdateLivesUI()
	{
		GameObject[] hearts_sprites = GameObject.FindGameObjectsWithTag ("FullHeart");

		if (hearts_sprites.Length <= 0)
			return;

		int cnt = 0;
		for (int i = 0; i < hearts_sprites.Length; ++i)
		{
			if (hearts_sprites[i].activeSelf) cnt++;
		}

		hearts_sprites [0].SetActive (false);
	}

	void UpdateInvulLayer()
	{
		if (!bInvul)
			return;
		
		if (Time.time - InvulStartTime > InvulSec) 
		{
			bInvul = false;
			actor_sprite.enabled = true;
			gameObject.layer = LayerMask.NameToLayer ("Player");
			animator.SetBool ("TakingDamage", false);
		}
	}

	IEnumerator UpdateFlashing()
	{
		if (bInvul) 
		{
			yield return new WaitForSeconds (0.05f);
			actor_sprite.enabled = false;
			yield return new WaitForSeconds (0.05f);
			actor_sprite.enabled = true;
			StartCoroutine (UpdateFlashing ());
		}
	}
}
