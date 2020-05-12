using System;
using SO.Audio;
using UnityEditor;
using UnityEngine;

namespace Inspector.CustomDrawers
{
	[CustomEditor(typeof(SoundEvent), true)]
	public class SoundEventEditor : Editor
	{
		[SerializeField]
		private AudioSource audioSource;

		private void OnEnable()
		{
			audioSource = EditorUtility
				.CreateGameObjectWithHideFlags(
					"Sound play",
					HideFlags.HideAndDontSave,
					typeof(AudioSource))
				.GetComponent<AudioSource>();
		}

		private void OnDisable()
		{
			DestroyImmediate(audioSource.gameObject);
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			if (GUILayout.Button("Play"))
			{
				((SoundEvent)target).Play(audioSource);

			}
			EditorGUI.EndDisabledGroup();
		}
	}
}