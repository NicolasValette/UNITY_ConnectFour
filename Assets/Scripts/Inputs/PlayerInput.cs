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
        private UIHandler _uiHandler;
        private bool _isGamePaused = false;

        // Update is called once per frame
        void Update()
        {
            var key = Keyboard.current;
            if (key.rightArrowKey.wasPressedThisFrame)
            {
                _pawnButtonHandler.RightColumn();
            }
            else if (key.leftArrowKey.wasPressedThisFrame)
            {
                _pawnButtonHandler.LeftColumn();
            }
            else if (key.enterKey.wasPressedThisFrame || key.spaceKey.wasPressedThisFrame)
            {
                _pawnButtonHandler.ActivateColumn();
            }

           
        }
    }
}