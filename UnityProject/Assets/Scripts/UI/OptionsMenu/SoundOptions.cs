using System.Collections;
using System.Collections.Generic;
using SoundManagers;
using UnityEngine;
using UnityEngine.UI;

namespace Unitystation.Options
{
    public class SoundOptions : MonoBehaviour
    {
        [SerializeField]
        private Slider ambientSlider = null;

        [SerializeField]
        private Toggle ttsToggle = null;

		[SerializeField]
		private Slider masterSlider = null;

		[SerializeField]
		private Slider soundEffectsSlider = null;

		void OnEnable()
        {
            Refresh();

		}

		public void OnSoundEffectsVolumeChange()
		{
			SoundEffectsManager.OnSoundEffectsVolumeChange(soundEffectsSlider.value);
		}

        public void OnAmbientVolumeChange()
        {
	        AmbienceSoundManager.AmbientVolume(ambientSlider.value);
        }

		public void OnMasterVolumeChange()
		{
			SoundManager.MasterVolume(masterSlider.value);
		}

		public void TTSToggle()
        {
            UIManager.ToggleTTS(ttsToggle.isOn);
        }

        void Refresh()
        {
            ambientSlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.AmbientVolumeKey);
            ttsToggle.isOn = PlayerPrefs.GetInt(PlayerPrefKeys.TTSToggleKey) == 1;
			masterSlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.MasterVolumeKey);
			soundEffectsSlider.value = PlayerPrefs.GetFloat(PlayerPrefKeys.SoundEffectsVolumeKey);
        }

        public void ResetDefaults()
        {
            ModalPanelManager.Instance.Confirm(
                "Are you sure?",
                () =>
                {
                    UIManager.ToggleTTS(false);
                    AmbienceSoundManager.AmbientVolume(1f);
					AudioListener.volume = 1;
					SoundEffectsManager.Volume = 1;
                    Refresh();
                },
                "Reset"
            );
        }
    }
}