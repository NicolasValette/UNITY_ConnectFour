using ConnectFour.BoardGame;
using ConnectFour.Inputs;
using System;
using UnityEngine;

namespace ConnectFour
{
    public class Hover : MonoBehaviour
    {
        [SerializeField]
        private int _column;                                // The column of the pawn
        #region EVENTS
        public static event Action<int> OnHover;            // Raised when we click on hovering element.
        #endregion

        private bool _isGameWin = false;
        private bool _isMouseActive = true;


        // Components we need later
        private Renderer _renderer;
        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _isGameWin = false;


            BoardGame.Grid.GameEnd += GameIsOver;
            PlayerInput.OnMouseOn += ReceiveMouseEvent;
            PlayerInput.OnMouseOff += HideMouseEvent;
        }

        private void OnDestroy()
        {
            BoardGame.Grid.GameEnd-= GameIsOver;
            PlayerInput.OnMouseOn -= ReceiveMouseEvent;
            PlayerInput.OnMouseOff -= HideMouseEvent;
        }

        public void ReceiveMouseEvent()
        {
            _isMouseActive = true;
            Exit();
        }
        public void HideMouseEvent()
        {
            _isMouseActive = false;
            Exit();
        }
        public void GameIsOver(PawnOwner winner)
        {
            _isGameWin= true;
            gameObject.SetActive(false);
        }
    
        public void Exit()
        {
            if (!_isGameWin)
            {
                if (_renderer != null && _renderer.enabled == true)
                {
                    _renderer.enabled = false;
                }
            }
        }
        private void OnMouseExit()
        {
            Exit();
        }
        public void Over()
        {
            if (!_isGameWin)
            {
                if (_renderer != null && _renderer.enabled == false)
                {
                    _renderer.enabled = true;
                }
            }
        }
        private void OnMouseOver()
        {
            if (_isMouseActive)
            {
                Over();
            }
        }
        public void Activate()
        {
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }
            OnHover?.Invoke(_column);
        }
        private void OnMouseUpAsButton()
        {
          Activate();
        }

        public void InitColumn (int column)
        {  
            _column = column; 
        }
    }
}