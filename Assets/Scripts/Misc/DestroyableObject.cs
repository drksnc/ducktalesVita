using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

	float fTimeStartDestroy = 0.0f;
	public AudioSource destroy_snd;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fTimeStartDestroy > 0.0f)
		{
			if (Time.time - fTimeStartDestroy > 0.2f) 
			{
				Object.Destroy (gameObject);
			}
		}
	}

	public void DestroyMe()
	{
		fTimeStartDestroy = Time.time;
		GetComponent<MeshRenderer> ().enabled = false;
		destroy_snd.Play ();
	}
}
