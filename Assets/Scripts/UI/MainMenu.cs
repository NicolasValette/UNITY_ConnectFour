using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.UI
{

    public class MainMenu : MonoBehaviour
    {
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}