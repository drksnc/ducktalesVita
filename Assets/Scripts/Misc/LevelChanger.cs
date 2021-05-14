using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

	// Use this for initialization
	public string SceneToGo = "";
	public Image BGBlack;
	bool bFadeToBlackStarted = false;

	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (bFadeToBlackStarted && BGBlack.GetComponent<Image> ().material.color.a <= 0.05f) {
			SceneManager.LoadScene (SceneToGo);
			bFadeToBlackStarted = false;
		}
			
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player") {
			BGBlack.GetComponent<BGOpacity> ().fade_in = true;
			bFadeToBlackStarted = true;
			SceneManager.LoadScene (SceneToGo);
		}
	}
		
}
