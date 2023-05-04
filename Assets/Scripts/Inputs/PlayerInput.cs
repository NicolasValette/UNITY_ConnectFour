using ConnectFour.AI;
using ConnectFour.BoardGame;
using ConnectFour.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConnectFour.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private PawnButtonHandler _pawnButtonHandler;
        [SerializeField]
        private ConnectFourAI _ai;
        [SerializeField]
        private UIHandler _uiHandler;

        #region EVENTS
        public static event Action OnMouseOff;
        public static event Action OnMouseOn;
        #endregion
        private bool _isGamePaused = false;

        public void HideMouse()
        {
            if (Cursor.visible == true)
            {
                Cursor.visible = false;
                OnMouseOff?.Invoke();
            }
           
        }
        // Update is called once per frame
        void Update()
        {
            var key = Keyboard.current;
            var mouse = Mouse.current;
            if (Cursor.visible == false && (mouse.delta.ReadValue() != Vector2.zero || mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame))
            {
                Cursor.visible = true;
                OnMouseOn?.Invoke();
            }
            if (key.cKey.wasPressedThisFrame)
            {
                _ai.PlayInsteadOfPlayer();
            }
            if (key.rightArrowKey.wasPressedThisFrame)
            {
                _pawnButtonHandler.RightColumn();
                HideMouse();
            }
            else if (key.leftArrowKey.wasPressedThisFrame)
            {
                _pawnButtonHandler.LeftColumn();
                HideMouse();
            }
            else if ((key.enterKey.wasPressedThisFrame || key.spaceKey.wasPressedThisFrame) && (mouse.delta.ReadValue() == Vector2.zero))
            {
                Debug.Log("Activate");
                _pawnButtonHandler.ActivateColumn();
            }
           
        }
    }
}