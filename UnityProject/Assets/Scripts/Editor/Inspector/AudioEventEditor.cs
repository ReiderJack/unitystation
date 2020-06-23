using Audio.Containers;
using UnityEditor;
using UnityEngine;

namespace CustomInspectors
{
	[CustomEditor(typeof(AudioEvent), true)]
	public class AudioEventEditor : Editor
	{
		private AudioSource audioSource;

		private void OnEnable()
		{
			audioSource = EditorUtility.CreateGameObjectWithHideFlags("Audio preview",HideFlags.HideAndDontSave,typeof(AudioSource)).GetComponent<AudioSource>();
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
				((AudioEvent)target).Play(audioSource);
			}

			if (GUILayout.Button("Stop"))
			{
				audioSource.Stop();
			}
			EditorGUI.EndDisabledGroup();
		}
	}
}