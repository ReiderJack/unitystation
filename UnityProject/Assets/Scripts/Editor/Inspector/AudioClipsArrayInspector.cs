using System;
using Audio.Containers;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace RoslynAnalyserSupport.Inspector
{
	[CustomEditor(typeof(AudioClipsArray))]
	public class AudioClipsArrayInspector : Editor
	{
		private SerializedProperty audioClips;
		private AudioSource audioSource;
		private int currentPickerWindow;

		private void OnEnable()
		{
			audioClips = serializedObject.FindProperty("audioClips");
			audioSource = EditorUtility.CreateGameObjectWithHideFlags("Audio preview",HideFlags.HideAndDontSave,typeof(AudioSource)).GetComponent<AudioSource>();
		}

		private void OnDisable()
		{
			DestroyImmediate(audioSource.gameObject);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.BeginVertical();

			for (int i = 0; i < audioClips.arraySize; i++)
			{

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField(i.ToString(),GUILayout.MaxWidth(20), GUILayout.MaxHeight(20));

				var objectReference = audioClips.GetArrayElementAtIndex(i).objectReferenceValue;
				EditorGUILayout.ObjectField(objectReference, typeof(AudioClip),false,
								GUILayout.MaxWidth(150),GUILayout.MaxHeight(20));

				if (GUILayout.Button("P", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					var audioClip = (AudioClip) objectReference;
					audioSource.clip = audioClip;

					audioSource.Play();
				}

				if (GUILayout.Button("S", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					audioSource.Stop();
				}

				if (GUILayout.Button("X", GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
				{
					audioClips.DeleteArrayElementAtIndex(i);
				}

				EditorGUILayout.EndHorizontal();
			}

			if (GUILayout.Button("Add clip"))
			{
				//create a window picker control ID
				currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive);

				//use the ID you just created
				EditorGUIUtility.ShowObjectPicker<AudioClip>(null,false,"",currentPickerWindow);
			}

			var curValuePick = EditorGUIUtility.GetObjectPickerObject();
			if (curValuePick != null)
			{
				Logger.Log("Cur Value");
				audioClips.InsertArrayElementAtIndex(0);
				audioClips.GetArrayElementAtIndex(0).objectReferenceValue = curValuePick;
			}
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical();

			GUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Add new clip",GUILayout.MaxWidth(80),GUILayout.MaxHeight(20));

			AudioClip newAudioClip = null;
			var newClip = EditorGUILayout.ObjectField(newAudioClip, typeof(AudioClip),false,
				GUILayout.MaxWidth(20),GUILayout.MaxHeight(20));

			if (newClip != null)
			{
				Logger.Log("Test");
				audioClips.InsertArrayElementAtIndex(0);
				audioClips.GetArrayElementAtIndex(0).objectReferenceValue = newClip;
			}

			GUILayout.EndHorizontal();

			serializedObject.ApplyModifiedProperties();
		}
	}
}