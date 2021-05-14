using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubworldButton : MonoBehaviour {

	bool bEnabled = true;
	public GameObject Cutscene;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll)
	{
		if (!bEnabled)
			return;
		
		if (coll.gameObject.tag == "Player") 
		{
			GetComponent<Animator>().Play ("Base Layer.push_button");
			Cutscene.GetComponent<cutscene_1> ().InitCutscene ();
		}
	}

	void OnAnimFinished()
	{
		bEnabled = false;
	}
}
