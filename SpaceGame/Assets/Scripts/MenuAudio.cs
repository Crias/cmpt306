﻿using UnityEngine;
using System.Collections;

public class MenuAudio : MonoBehaviour {

	private static MenuAudio instance = null;
	public static bool isPlaying = true;

	public static MenuAudio Instance {
		get { return instance; }
	}

	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Stop audio
	public void StopAudio() {
		instance.GetComponent<AudioSource>().Stop();
	}

	// Play audio
	public void PlayAudio() {
		instance.GetComponent<AudioSource>().Play();
	}



}
