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
        public PawnOwner ActivePlayer                                       // Return the player who is playing
        {
            get
            {
                return _isPlayer1Turn?PawnOwner.Player1:PawnOwner.Player2;
            }
        }
        public PawnOwner PassivePlayer                                      // Return the player who is waiting
        {
            get
            {
                return _isPlayer1Turn ? PawnOwner.Player2 : PawnOwner.Player1;
            }
        }
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