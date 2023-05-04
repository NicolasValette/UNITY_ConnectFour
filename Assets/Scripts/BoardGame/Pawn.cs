using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConnectFour.BoardGame
{
    public  enum PawnOwner
    {
        None,
        PlayerRed,
        PlayerYellow
    }
    // Class to describe pawn behaviour
    public class Pawn : MonoBehaviour
    {
        [SerializeField]
        private float _fallingTime = 1f;
        [SerializeField]
        private ParticleSystem _winParticleEffect;
        [SerializeField]
        private AudioClip _winSound;

        public IEnumerator Move (float startYPos, float targetYpos, float timeDuration, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            AudioSource audiosource = GetComponent<AudioSource>();
            if (audiosource != null)
            {
                audiosource.Play();
            }

            float timeElapsed = 0f;
            while (timeElapsed < timeDuration)
            {
                transform.localPosition = new Vector3 (transform.position.x, Mathf.Lerp(startYPos, targetYpos, timeElapsed / timeDuration), transform.position.z);
                timeElapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.localPosition = new Vector3(transform.localPosition.x, targetYpos, transform.localPosition.z);
            if (audiosource== null)
            {

            audiosource.Stop();
            }

            callbackAfterFalling(lastRowPlayed, lastColPlayed);
        }
        public void Fall(float targetPos, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            StartCoroutine(Move(transform.position.y, targetPos, ((_fallingTime != 0) ? _fallingTime : 1f), lastRowPlayed, lastColPlayed, callbackAfterFalling));
        }
        public void PlayWinEffect()
        {
            if (_winParticleEffect != null)
            {
                _winParticleEffect.Play();
            }
            AudioSource audiosource = GetComponent<AudioSource>();
            if (audiosource != null )
            {
                audiosource.clip = _winSound;
                audiosource.Play();
            }
        }

    }
}
