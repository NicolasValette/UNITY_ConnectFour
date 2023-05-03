using ConnectFour.BoardGame;
using ConnectFour.Game;
using TMPro;
using UnityEngine;

namespace ConnectFour.UI
{
    public class UIHandler : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _winText;
        [SerializeField]
        private GameObject _endGameUIElements;
        [SerializeField]
        private BoardGame.Grid _grid;
        [SerializeField]
        private GameObject _chooseButtons;
        // Start is called before the first frame update
        void Start()
        {
            _endGameUIElements.SetActive(false);
        }
        private void OnEnable()
        {
            BoardGame.Grid.GameEnd += DisplayWinner;
            TurnManager.OnColorChoose += ToggleChooseButton;
        }
        private void OnDisable()
        {
            BoardGame.Grid.GameEnd -= DisplayWinner;
            TurnManager.OnColorChoose -= ToggleChooseButton;
        }
        public void DisplayWinner (PawnOwner player)
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

        public void ToggleChooseButton(PawnOwner playerChoice)
        {
            _chooseButtons.SetActive(!_chooseButtons.activeSelf);
        }
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