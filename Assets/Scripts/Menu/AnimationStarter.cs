using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStarter : MonoBehaviour {
	bool bAnimStarted = false;
	bool bSoundPlayed = false;
	public AudioSource m_start_sound = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!bSoundPlayed) {
			if (Time.time > 3) {
				m_start_sound.Play ();
				bSoundPlayed = true;
			}
		}
	
		if (!bAnimStarted) {
			if (Time.time > 4) {
				StartStartupGyroAnimation ();
				bAnimStarted = true;
			}
		}

		if (Time.time > 15)
			gameObject.SetActive (false);
		
	}

	void StartStartupGyroAnimation()
	{
		Animator anim = GetComponent<Animator> ();
		anim.Play ("Base Layer.gyro_heli_startup");
	}
}
