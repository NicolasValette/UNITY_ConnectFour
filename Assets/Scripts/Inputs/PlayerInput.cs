using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConnectFour.Inputs
{
    public class PlayerInput : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            var key = Keyboard.current;
            if (key.escapeKey.wasPressedThisFrame)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }
}