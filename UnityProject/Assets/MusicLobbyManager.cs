using System;
using System.Collections;
using System.Collections.Generic;
using SO.Audio;
using UnityEngine;

public class MusicLobbyManager : MonoBehaviour
{

	private void OnEnable()
	{
		SOManagerMusic.Instance.currentLobbyAudioSource = GetComponent<AudioSource>();
	}

	private void OnDisable()
	{
		SOManagerMusic.Instance.currentLobbyAudioSource = null;
	}
}
