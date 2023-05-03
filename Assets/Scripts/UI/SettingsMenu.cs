using ConnectFour.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.UI
{


    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private AudioPlayer _audioPlayer;
        [SerializeField]
        private Slider _musicSlider;
        [SerializeField]
        private Slider _sfxSlider;
        private void Start()
        {
            _musicSlider.value = _audioPlayer.GetMusicVolume();
            _sfxSlider.value = _audioPlayer.GetSFXVolume();
        }
        public void SetMusicVolume(float musicVolume)
        {
            _audioPlayer.SetMusicVolume(musicVolume); 
        }
        public void SetSFXVolume(float sfxVolume)
        {
            _audioPlayer.SetSFXVolume(sfxVolume);
        }
        

    }
}