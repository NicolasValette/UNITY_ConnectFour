using ConnectFour.BoardGame;
using ConnectFour.Game;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ConnectFour.UI
{
    public class UIHandler : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField]
        private TMP_Text _winText;
        [SerializeField]
        private GameObject _endGameUIElements;
        [SerializeField]
        private GameObject _pauseMenuElements;
        [SerializeField]
        private GameObject _chooseButtons;
        [SerializeField]
        private TMP_Text _chooseText;
        [SerializeField]
        private GameObject _helpPanel;
        [SerializeField]
        private TMP_Text _beginnerText;
        [SerializeField]
        private float _fadeDuration = 5f;
        [Header("References")]
        [SerializeField]
        private BoardGame.Grid _grid;
        [SerializeField]
        private TurnManager turnManager;
       
        // Start is called before the first frame update
        void Start()
        {
            _endGameUIElements.SetActive(false);
            _helpPanel.SetActive(false);
            _beginnerText.enabled = false;
        }
        private void OnEnable()
        {
            BoardGame.Grid.GameEnd += DisplayWinner;
            TurnManager.OnColorChoose += ToggleChoose;
            TurnManager.OnBeginnerChoose += DisplayBeginner;

        }
        private void OnDisable()
        {
            BoardGame.Grid.GameEnd -= DisplayWinner;
            TurnManager.OnColorChoose -= ToggleChoose;
            TurnManager.OnBeginnerChoose -= DisplayBeginner;
        }
        public void DisplayWinner(PawnOwner player)
        {
            if (player == PawnOwner.None)
            {
                _winText.text = "DRAW !";

            }
            else
            {
                _winText.text = player.ToString() + " WINS !!";
            }
            _endGameUIElements.SetActive(true);
        }

        public void ToggleChoose(PawnOwner playerChoice)
        {
            _chooseText.enabled = false;
        }
        public void ToggleHelpPanel()
        {
            _helpPanel.SetActive(!_helpPanel.activeSelf);
        }
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void DisplayBeginner(PawnOwner beginner)
        {
            _beginnerText.enabled = true;
            _beginnerText.text = beginner.ToString() + " starts the game !";
            StartCoroutine(FadeText(_beginnerText, _fadeDuration));
        }
        public IEnumerator FadeText(TMP_Text textToFade, float duration)
        {
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                textToFade.alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
