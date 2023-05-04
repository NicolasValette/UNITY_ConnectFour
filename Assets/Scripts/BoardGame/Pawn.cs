using System;
using System.Collections;
using UnityEngine;

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
        private ParticleSystem _winParticleEffect;
        [SerializeField]
        private AudioClip _winSound;
        [SerializeField]
        private float _fallingTime = 1f;

        public IEnumerator Move (float startYPos, float targetYpos, float timeDuration, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            // First, we play the falling sound
            AudioSource audiosource = GetComponent<AudioSource>();
            if (audiosource != null)
            {
                audiosource.Play();
            }

            //Then, we lerp the position to make a smooth falling
            float timeElapsed = 0f;
            while (timeElapsed < timeDuration)
            {
                transform.localPosition = new Vector3 (transform.position.x, Mathf.Lerp(startYPos, targetYpos, timeElapsed / timeDuration), transform.position.z);
                timeElapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            // We set position to the final position to prevent misplacement due to missing frames
            transform.localPosition = new Vector3(transform.localPosition.x, targetYpos, transform.localPosition.z);

            //And, we top the sound of falling
            if (audiosource== null)
            {
            audiosource.Stop();
            }

            // Call the callback, what we must do after falling
            callbackAfterFalling(lastRowPlayed, lastColPlayed);
        }
        /// <summary>
        /// Make a smooth falling of the pawn.
        /// </summary>
        /// <param name="targetPos">The target position in the grid.</param>
        /// <param name="lastRowPlayed">Row coord of the pawn.</param>
        /// <param name="lastColPlayed">Column coord of the pawn.</param>
        /// <param name="callbackAfterFalling">Callback called after falling.</param>
        public void Fall(float targetPos, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            StartCoroutine(Move(transform.position.y, targetPos, ((_fallingTime != 0) ? _fallingTime : 1f), lastRowPlayed, lastColPlayed, callbackAfterFalling));
        }

        /// <summary>
        /// Play the VFX & SFX of the pawn when it's the winning pawn.
        /// </summary>
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
