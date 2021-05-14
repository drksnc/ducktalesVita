using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutscene_0 : MonoBehaviour {

	// Use this for initialization

	ActorInput actor_input;
	Animator actor_animator;
	CameraController Camera;

	public RectTransform TopBorder;
	public RectTransform BotBorder;

	float BottomNormalY = -645.0f;
	float TopNormalY = 645.0f;

	float BottomBorderYOff = -476.0f;
	float TopBorderYOff = 496.0f;

	public TextParser cutscene_array;
	public Text text_field;

	bool bRunning = false;



	public Animator bomber_animator;
	public Animator baggy_animator;

	public Transform cam_target_0;
	public AudioSource HueyLine_0;
	public AudioSource ScroogeLine_0;
	public AudioSource HueyLine_1;
	public AudioSource BaggyLine_0;
	public AudioSource ScroogeLine_1;
	public AudioSource HueyLine_2;
	public AudioSource ScroogeLine_2;

	public GameObject Barrier;

	void Start () {
		actor_animator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		actor_input = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorInput> ();
		Camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player") 
		{
			InitCutscene ();	
		}
	}

	void InitCutscene()
	{
		actor_input.DisableInput ();
		GetComponent<Collider> ().enabled = false;

		StartCoroutine (EnableBorders ());

		bRunning = true;
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

		StartDialogSequence ();
	}

	IEnumerator ShowText(int line)
	{
		string current_line = cutscene_array.GetCutsceneLine ("cutscene_0", line);
		text_field.text = "";

		for (int i = 0; i < current_line.Length; ++i) {
			text_field.text += current_line [i];
			yield return new WaitForSeconds (0.05f); 
		}

		yield return new WaitForSeconds (1.5f); 

		switch (line) {
		case 0:
			Action (0); break;
		case 1:
			Action (1); break;
		case 2:
			Action (2); break;
		case 3:
			Action (3); break;
		case 4:
			Action (4); break;
		case 5: 
			Action (5); break;
		case 6: 
			Action (6); break;
		default:
			break;
		}
	}

	IEnumerator MoveScroogeAndAnswer()
	{
		actor_input.MoveRight ();

		yield return new WaitForSeconds(0.1f);

		actor_input.Jump();

		yield return new WaitForSeconds (0.9f);

		actor_input.StopManualMove ();

		ScroogeLine_0.Play ();
		StartCoroutine(ShowText (1));

		yield return new WaitForSeconds (0.5f);
	
		actor_animator.SetLayerWeight (1, 2.0f);
	}

	IEnumerator ScroogeAvoidTrap()
	{
		actor_input.CanFlipSprite (false);
		bomber_animator.Play ("Base Layer.bomber_cutscene");
		actor_animator.SetLayerWeight (1, 2.0f);
		actor_input.Jump ();
		actor_input.MoveLeft ();
	
		yield return new WaitForSeconds(0.2f);

		actor_input.Fall ();
		actor_input.StopManualMove ();

		while (!actor_input.IsOnGround ())
			yield return new WaitForSeconds (0.1f);

		actor_animator.SetLayerWeight (0, 0.0f);
		actor_animator.SetLayerWeight (1, 0.0f);

		StartCoroutine(BaggySayPharase());
	}

	IEnumerator BaggySayPharase()
	{
		yield return new WaitForSeconds (0.5f);
		BaggyLine_0.Play ();
		StartCoroutine(ShowText (3));
		baggy_animator.Play ("Base Layer.idle_0");
		Barrier.SetActive (true);
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

		Barrier.SetActive (true);
		actor_animator.SetLayerWeight (0, 0.0f);
		actor_animator.SetLayerWeight (1, 0.0f);
		actor_input.CanFlipSprite (true);
		actor_input.EnableInput ();
		Camera.SetTarget(cam_target_0, 0.02f);

		//Camera.ResetTarget ();

		bRunning = false;
		gameObject.SetActive (false);
	}

	public void EndCutScene()
	{
		StopAllCoroutines ();
		StartCoroutine(FinishCutScene());
	}

	void StartDialogSequence()
	{
		HueyLine_0.Play ();
		StartCoroutine(ShowText (0));
	}

	void Action(int id)
	{
		switch (id) {
		case 0:
			{
				//Move camera to Huey, go to point, say phrase
				Camera.SetTarget(cam_target_0, 0.02f);
				StartCoroutine (MoveScroogeAndAnswer ());
			}break;
		case 1:
			{
				actor_animator.SetLayerWeight (0, 0.0f);
				actor_animator.SetLayerWeight (1, 0.0f);

				StartCoroutine(ShowText (2));
				HueyLine_1.Play ();
			}
			break;
		case 2:
			{
				StartCoroutine (ScroogeAvoidTrap ());
			}
			break;
		case 3:
			{
				baggy_animator.Play ("Base Layer.idle");

				ScroogeLine_1.Play ();
				StartCoroutine(ShowText (4));
				actor_animator.SetLayerWeight (1, 2.0f);

			}
			break;
		case 4:
			{
				actor_animator.SetLayerWeight (0, 0.0f);
				actor_animator.SetLayerWeight (1, 0.0f);

				HueyLine_2.Play ();
				StartCoroutine (ShowText (5));
			}
			break;
		case 5:
			{
				ScroogeLine_2.Play ();
				StartCoroutine (ShowText (6));
			}
		break;
		case 6:
			{
				StartCoroutine (FinishCutScene ());
			}
			break;
		}
	}
}
