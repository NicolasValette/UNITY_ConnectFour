using UnityEngine;
using UnityEngine.Audio;

namespace ConnectFour.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer _audioMixer;
        // Start is called before the first frame update
        void Start()
        {
            // We get user preference for music volume
            _audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            _audioMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
        }
        public float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat("MusicVolume");

        }
        public float GetSFXVolume()
        {
            return PlayerPrefs.GetFloat("SFXVolume");

        }
        public void SetMusicVolume(float volume)
        {
            _audioMixer.SetFloat("MusicVolume", volume);            
            PlayerPrefs.SetFloat("MusicVolume", volume);             // We save user preferences
        }
        public void SetSFXVolume(float volume)
        {
            _audioMixer.SetFloat("SFXVolume", volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);              // We save user preferences
        }
    }
}
