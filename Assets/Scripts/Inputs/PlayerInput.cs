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
    /// <summary>
    /// Class to hold the player input using the new Input System.
    /// </summary>
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

        /// <summary>
        /// Method to hide mouse when we use the keyboard
        /// </summary>
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
                // Cheat code to let AI play instead of player.
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
                _pawnButtonHandler.ActivateColumn();
            }
           
        }
    }
}