using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Audio
{

    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
