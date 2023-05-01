using ConnectFour.BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Game
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pawnButtons;

        private bool _isPlayer1Turn = true;
        private bool _isWin = false;
        public bool IsWin { get => _isWin; }
        public bool IsPlayer1Turn { get => _isPlayer1Turn; }
        // Start is called before the first frame update
        void Start()
        {
            
        }
        private void OnEnable()
        {
            Grid.SwitchTurn += SwitchTurn;
            Grid.GameEnd += GameWin;
        }
        private void OnDisable()
        {
            Grid.SwitchTurn -= SwitchTurn;
            Grid.GameEnd -= GameWin;
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void SwitchTurn()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
            _pawnButtons.SetActive(!_pawnButtons.activeSelf);
        }
        public void GameWin(PawnOwner player)
        {
            _isWin = true;
            _pawnButtons.SetActive(false);
        }
    }
}