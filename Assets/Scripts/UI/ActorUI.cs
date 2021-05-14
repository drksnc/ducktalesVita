using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorUI : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float fps = 1.0f / Time.fixedUnscaledDeltaTime;
		GetComponent<Text> ().text = "FPS " + fps;
	}
}
