using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontScalingAnim : MonoBehaviour {
	bool bGrowing = true;
	RectTransform rect = null;
	float start_scale_x = 0.0f;
	float start_scale_y = 0.0f;
	float scale_diff = 0.1f;

	// Use this for initialization
	void Start () {
		rect = GetComponent <RectTransform> ();
		start_scale_x = rect.localScale.x;
		start_scale_y = rect.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (bGrowing) {
			if (rect.localScale.x < start_scale_x + scale_diff || rect.localScale.y < start_scale_y + scale_diff) {
				Vector3 scale = rect.localScale;

				scale.x += Time.deltaTime * 0.1f;
				scale.y += Time.deltaTime * 0.1f;

				rect.localScale = scale;
			}
			else
				bGrowing = false;
		} else 
		{
			if (rect.localScale.x > start_scale_x || rect.localScale.y > start_scale_y) {
				Vector3 scale = rect.localScale;

				scale.x -= Time.deltaTime * 0.1f;
				scale.y -= Time.deltaTime * 0.1f;

				rect.localScale = scale;
			}
			else
				bGrowing = true;
		}
	}
}
