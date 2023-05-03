using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.BoardGame
{
    public  enum PawnOwner
    {
        None,
        Player1,
        Player2
    }
    // Class to describe pawn behaviour
    public class Pawn : MonoBehaviour
    {
        private int _columnNumber;
        private int _rowNumber;
        private bool _isFalling = false;
        //private float _targetYPosition;
        private float _fallingTime = 1f;
        private AudioSource _audioSource;

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() 
        {

        }
        public IEnumerator Move (float startYPos, float targetYpos, float timeDuration, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            AudioSource audiosource = GetComponent<AudioSource>();
            audiosource.Play();

            float timeElapsed = 0f;
            while (timeElapsed < timeDuration)
            {
                transform.localPosition = new Vector3 (transform.position.x, Mathf.Lerp(startYPos, targetYpos, timeElapsed / timeDuration), transform.position.z);
                timeElapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            audiosource.Stop();
            callbackAfterFalling(lastRowPlayed, lastColPlayed);
        }
        public void Fall(float targetPos, int lastRowPlayed, int lastColPlayed, Action<int, int> callbackAfterFalling)
        {
            Debug.Log("Move from " + transform.position.y + " to " + targetPos);
            StartCoroutine(Move(transform.position.y, targetPos, ((_fallingTime != 0) ? _fallingTime : 1f), lastRowPlayed, lastColPlayed, callbackAfterFalling));
        }
        // Set this pawn in the grid at the correct column and the row where the pawn must fall
        public void PutPawnOnGrid(int row, int column)
        {
            _columnNumber = column;
            _rowNumber = row;
        }
    }
}
