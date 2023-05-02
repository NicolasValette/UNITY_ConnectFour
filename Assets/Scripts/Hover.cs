using ConnectFour.BoardGame;
using ConnectFour.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
    public class Hover : MonoBehaviour
    {
        [SerializeField]
        private int _column;
        #region EVENTS
        public static event Action<int> OnHover;
        #endregion

        private bool _isMouseOver;
        private bool _isGameWin = false;

        // Components we need later
        private Renderer _renderer;
        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _isGameWin = false;
        }
        private void OnEnable()
        {
            Grid.GameEnd += GameIsOver;
        }
        private void OnDisable()
        {
            Grid.GameEnd-= GameIsOver;
        }
        public void GameIsOver(PawnOwner winner)
        {
            _isGameWin= true;
            gameObject.SetActive(false);
        }
        private void OnMouseExit()
        {
            if (!_isGameWin && _isMouseOver)
            {
                _isMouseOver = false;
                if (_renderer != null)
                {
                    _renderer.enabled = false;
                }
            }
        }
        private void OnMouseOver()
        {
            if (!_isGameWin && !_isMouseOver)
            {
                _isMouseOver = true;
                if (_renderer != null)
                {
                    _renderer.enabled = true;
                }
            }

        }
        private void OnMouseUpAsButton()
        {
            OnHover?.Invoke(_column);
        }

        public void InitColumn (int column)
        {  
            _column = column; 
        }
    }
}