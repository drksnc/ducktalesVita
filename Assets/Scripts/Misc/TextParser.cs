using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class TextParser : MonoBehaviour {

	string path;
	Dictionary<string, List<string>> cutscene_texts;

	// Use this for initialization
	void Start () {
		path = "Text/Cutscenes/" + SceneManager.GetActiveScene().name + "/cutscene_";
		cutscene_texts = new Dictionary<string, List<string>>();

		int i = 0;
		TextAsset assets = (TextAsset)Resources.Load (path + i.ToString(), typeof(TextAsset));
	
		while (assets != null) 
		{
			List<string> lines = new List<string> ();
			string[] arrayStr = assets.text.Split (char.Parse("\n"));

			foreach (string line in arrayStr) {
				lines.Add (line);
			}

		
			cutscene_texts.Add (assets.name, lines);
			i++;
			assets = (TextAsset)Resources.Load (path + i.ToString (), typeof(TextAsset));
		}

	}

	public string GetCutsceneLine(string cutscene_name, int line)
	{
		return cutscene_texts [cutscene_name] [line];
	}
	
	// Update is called once per frame
	void Update () {

	}
}
