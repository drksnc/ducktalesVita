using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject Cutscenes;
	ActorInput actor_input;
	// Use this for initialization
	void Start () {
		actor_input = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorInput> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.JoystickButton0))
		{
			actor_input.PauseGame(false);
			Cutscenes.SendMessage ("EndCutScene");
		}
	}
	}
