using ConnectFour.BoardGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ConnectFour.UI
{
    public class UIHandler : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text _winText;
        // Start is called before the first frame update
        void Start()
        {
            _winText.enabled = false;
        }
        private void OnEnable()
        {
            Grid.GameWin += DisplayWinner;
        }
        private void OnDisable()
        {
            Grid.GameWin -= DisplayWinner;
        }
        public void DisplayWinner (PawnOwner player)
        {
            _winText.text = player.ToString() + " WINS !!";
            _winText.enabled = true;
        }
    }
}