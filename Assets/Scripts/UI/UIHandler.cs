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
            Grid.GameEnd += DisplayWinner;
        }
        private void OnDisable()
        {
            Grid.GameEnd -= DisplayWinner;
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
            _winText.enabled = true;
        }
    }
}