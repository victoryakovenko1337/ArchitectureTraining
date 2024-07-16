﻿using UnityEngine;

public class ChangeScene : MonoBehaviour {

	public string NameScene = "GameScene";

	public void ChangueScene(){
		GetComponent<AudioSource>().Play();
		Invoke("GameNameScene", GetComponent<AudioSource>().clip.length);
	}


	void GameNameScene(){
		Application.LoadLevel (NameScene);
	}
}
