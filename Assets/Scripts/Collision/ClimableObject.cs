using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimableObject : MonoBehaviour {

	ActorInput actor;
	// Use this for initialization
	void Start () {
		actor = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorInput>();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player") 
		{
			actor.collider_we_climbing = (BoxCollider)GetComponent<Collider>();
		}
	}
}
