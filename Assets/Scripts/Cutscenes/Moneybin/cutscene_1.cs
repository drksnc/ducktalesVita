using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutscene_1 : MonoBehaviour {

	ActorInput actor_input;
	Animator actor_animator;
	CameraController Camera;

	public RectTransform TopBorder;
	public RectTransform BotBorder;

	public SpriteRenderer baggy_sprite;

	float BottomNormalY = -645.0f;
	float TopNormalY = 645.0f;

	float BottomBorderYOff = -476.0f;
	float TopBorderYOff = 496.0f;

	public TextParser cutscene_array;
	public Text text_field;

	bool bRunning = false;
	bool bBeagleRun = false;

	public AudioSource BaggyLine_0;
	public AudioSource BaggyLine_1;
	public AudioSource ScroogeLine_0;
	public AudioSource Huey_Line_0;
	public AudioSource ScroogeLine_1;
	public AudioSource Huey_Line_1;
	public AudioSource ScroogeLine_2;
	public AudioSource Huey_Line_2;

	public Transform cam_point;

	public Animator Door;
	public Animator Bomber;
	public Animator Baggy;
	public Animator Huey;

	public GameObject Border;
	public GameObject FuzzyStar;

	// Use this for initialization
	void Start () {
		actor_animator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		actor_input = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorInput> ();
		Camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (bBeagleRun)
			Baggy.gameObject.transform.position += Vector3.right * 8 * Time.deltaTime;
	}

	IEnumerator PlayBomberAnim()
	{
		yield return new WaitForSeconds (1.5f);

		Door.Play ("Base Layer.cutscene_open");

		yield return new WaitForSeconds (0.4f);

		Bomber.Play ("Base Layer.bomber_cutscene_1");

		yield return new WaitForSeconds (1.2f);

		FuzzyStar.SetActive (true);
		Baggy.Play ("Base Layer.stun");
		Border.SetActive (false);

		yield return new WaitForSeconds (0.3f);

		BaggyLine_1.Play ();
		StartCoroutine(ShowText (0));

		actor_input.Jump ();
		actor_input.MoveRight ();

		yield return new WaitForSeconds (0.8f);

		actor_input.StopManualMove ();

		FinishCutScene ();
	}

	IEnumerator BaggyRun()
	{
		bBeagleRun = true;
		Baggy.Play ("Base Layer.run");
		baggy_sprite.flipX = false;
		FuzzyStar.SetActive (false);

		yield return new WaitForSeconds (0.9f);

		actor_animator.SetLayerWeight (0, 0.0f);
		actor_animator.SetLayerWeight (1, 0.0f);

		bBeagleRun = false;

		GameObject.Destroy (Baggy.gameObject);

		Huey.Play ("Base Layer.jump");

		Huey_Line_0.Play ();
		StartCoroutine (ShowText (2));

	}

	public void InitCutscene()
	{
		actor_input.DisableInput ();
		StartCoroutine (EnableBorders ());
		bRunning = true;

		BaggyLine_0.Play ();
		Camera.SetTarget (cam_point, 0.05f);

		StartCoroutine (PlayBomberAnim ());
	}
		
	IEnumerator EnableBorders()
	{
		while (TopBorder.anchoredPosition.y > TopBorderYOff &&
			BotBorder.anchoredPosition.y < BottomBorderYOff) 
		{
			Vector3 top = TopBorder.anchoredPosition;
			top.y -= 1;
			TopBorder.anchoredPosition = top;

			Vector3 bot = BotBorder.anchoredPosition;
			bot.y += 1;
			BotBorder.anchoredPosition = bot;

			yield return new WaitForSeconds (0.005f); 
		}
	}

	IEnumerator FinishCutScene()
	{
		if (!bRunning)
			yield return 0;

		text_field.text = "";

		while (TopBorder.anchoredPosition.y < TopNormalY &&
			BotBorder.anchoredPosition.y > BottomNormalY) 
		{
			Vector3 top = TopBorder.anchoredPosition;
			top.y += 1;
			TopBorder.anchoredPosition = top;

			Vector3 bot = BotBorder.anchoredPosition;
			bot.y -= 1;
			BotBorder.anchoredPosition = bot;

			yield return new WaitForSeconds (0.005f); 
		}
			
		actor_animator.SetLayerWeight (0, 0.0f);
		actor_animator.SetLayerWeight (1, 0.0f);
		actor_animator.SetBool ("CutScene_Strict", false);
		actor_animator.SetBool ("CutScene_Strict_idle", false);

		actor_input.CanFlipSprite (true);
		actor_input.EnableInput ();
		Camera.ResetTarget ();
		gameObject.SetActive (false);

		if (Baggy != null)
			Baggy.gameObject.SetActive (false);

		bRunning = false;
	}


	IEnumerator ShowText(int line)
	{
		string current_line = cutscene_array.GetCutsceneLine ("cutscene_1", line);
		text_field.text = "";

		for (int i = 0; i < current_line.Length; ++i) {
			text_field.text += current_line [i];
			yield return new WaitForSeconds (0.05f); 
		}

		yield return new WaitForSeconds (1.5f); 

		switch (line) {
		case 0:
			Action (0);
			break;
		case 1:
			Action (1);
			break;
		case 2:
			Action (2);
			break;
		case 3:
			Action (3);
			break;
		case 4:
			Action (4);
			break;
		case 5:
			Action (5);
			break;
		case 6:
			Action (6);
			break;
		default:
			break;
		}
	}

	void Action(int id)
	{
		switch (id) {
		case 0:
			actor_animator.SetLayerWeight (0, 0.0f);
			actor_animator.SetLayerWeight (1, 1.0f);
			actor_animator.SetBool ("CutScene_Strict", true);

			ScroogeLine_0.Play ();
			StartCoroutine (ShowText (1));
		
			break;
		case 1:
			StartCoroutine (BaggyRun());
			break;
		case 2:
			ScroogeLine_1.Play ();
			StartCoroutine (ShowText (3));
			break;
		case 3:
			Huey_Line_1.Play ();
			StartCoroutine (ShowText (4));
			break;
		case 4:
			ScroogeLine_2.Play ();
			StartCoroutine (ShowText (5));
			break;
		case 5:
			Huey_Line_2.Play ();
			StartCoroutine (ShowText (6));
			break;
		case 6:
			StartCoroutine (FinishCutScene());
			break;
		default:
			break;
		}
	}
}
