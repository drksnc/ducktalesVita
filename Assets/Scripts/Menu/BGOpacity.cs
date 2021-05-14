using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGOpacity : MonoBehaviour {
	public float transitionTime = 0.5f;
	public float delayTime = 2.5f;
	public bool fade_in = false;

	Image render;
	// Use this for initialization
	void Start () {
		render = GetComponent<Image> ();
		Color col = render.material.color;
		col.a = 1.0f;
		render.material.color = col;
	}

	// Update is called once per frame
	void Update () {

		if (!fade_in) {
			Color col = render.material.color;

			if (col.a <= 0.01f) {
				return;
			}

			if (Time.time < delayTime)
				return;
		
			col.a -= Time.deltaTime * 0.5f;
			render.material.color = col;
		} 
		else 
		{
			Color col = render.material.color;

			if (col.a >= 0.99f) {
				return;
			}

			if (Time.time < delayTime)
				return;

			col.a += Time.deltaTime * 0.5f;
			render.material.color = col;
		}
	}
}
