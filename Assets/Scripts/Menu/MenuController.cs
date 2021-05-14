using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public GameObject press_start_cap;
	public GameObject main_menu_cap;
	public GameObject cursor;

	public GameObject button_accept;
	public GameObject button_back;

	public AudioSource start_sound;
	public AudioSource select_sound;

	private int idx_selected = 0;
	private int idx_prev = 0;
	private const float cursor_menu_offset = 165.0f;

	//0 - Press Start Caption
	//1 - Main Menu
	private int idx_menu = 0;

	// Use this for initialization
	void Start () {
		cursor.SetActive (false);
		press_start_cap.SetActive (false);
		//Application.targetFrameRate = 30;
	}
	
	// Update is called once per frame
	void Update () {
		switch (idx_menu) 
		{
		case 0:
			UpdatePressStartCaption ();
			break;
		case 1:
			UpdateMainMenu ();
			break;
		}
	}

	void UpdatePressStartCaption()
	{
		if (Time.time > 3.0f && !press_start_cap.activeSelf) {
			press_start_cap.SetActive (true);
		}

		if (main_menu_cap.activeSelf)
			main_menu_cap.SetActive (false);

		if (button_accept.activeSelf || button_back.activeSelf) {
			button_accept.SetActive (false);
			button_back.SetActive (false);
		}
			
		if (!press_start_cap.activeSelf)
			return;

		if (Input.GetKeyDown (KeyCode.JoystickButton0) || Input.GetKeyDown (KeyCode.JoystickButton7)) {
			idx_menu = 1;
		}
	}

	void UpdateMainMenu()
	{
		if (press_start_cap != null)
			press_start_cap.SetActive (false);

		//Init actual main menu here
		if (!main_menu_cap.activeSelf) {
			start_sound.Play ();
			main_menu_cap.SetActive (true);
		}

		if (!button_accept.activeSelf || !button_back.activeSelf) {
			button_accept.SetActive (true);
			button_back.SetActive (true);
		}

		if (!cursor.activeSelf)
			cursor.SetActive (true);

		if (Input.GetKeyDown (KeyCode.JoystickButton10)) { //down
			if (idx_selected + 1 < 4)
				idx_selected++;
			else
				idx_selected = 0;

			select_sound.Play ();

		} else if (Input.GetKeyDown (KeyCode.JoystickButton8)) { //up
			if (idx_selected > 0)
				idx_selected--;
			else
				idx_selected = 3;

			select_sound.Play ();

		} else if (Input.GetKeyDown (KeyCode.JoystickButton1)) { //circle
			idx_menu = 0;
			idx_selected = 0;
			cursor.SetActive (false);
		}

		Text[] menu_caps = main_menu_cap.GetComponentsInChildren<Text> ();
		for (int i = 0; i < menu_caps.Length; ++i) {
			if (idx_selected == i) {
				menu_caps [i].color = new Color32 (255, 255, 176, 255); 
				Vector3 target_pos = menu_caps [i].GetComponent<RectTransform> ().anchoredPosition;
				target_pos.x -= cursor_menu_offset;
				cursor.GetComponent<RectTransform>().anchoredPosition = target_pos;

				if (idx_selected != idx_prev) {
					StartCursorSelectAnim ();
					idx_prev = idx_selected;
				}

			} else {
				menu_caps [i].color = new Color32 (237, 188, 0, 255); 
			}
		}

		if (Input.GetKeyDown(KeyCode.JoystickButton0)) // cross
		{
			start_sound.Play ();

			switch (idx_selected) {
			case 0:
				SceneManager.LoadScene ("test_scene");//new game
				break;
			case 1: //options
				break;
			case 2: //extras
				break;
			case 3://exit
				Application.Quit();
				break;
			}
		}
	}

	void StartCursorSelectAnim()
	{
		Animator anim = cursor.GetComponent<Animator> ();
		anim.Play ("Base Layer.cursor_select");
	}
}
