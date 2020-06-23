using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Audio.Containers;
using UnityEngine;
using UnityEngine.Events;

public class TestClipScript : MonoBehaviour, ICheckedInteractable<HandApply>
{
	public MyTestEvent testEvent;
	public bool WillInteract(HandApply interaction, NetworkSide side)
	{
		//start with the default HandApply WillInteract logic.
		if (!DefaultWillInteract.Default(interaction, side)) return false;

		return true;
	}

	public void ServerPerformInteraction(HandApply interaction)
	{
		//AudioSourceData.PlayServer(gameObject);
		testEvent?.Invoke(gameObject);
	}
}
[System.Serializable]
public class MyTestEvent : UnityEvent<GameObject> { }
