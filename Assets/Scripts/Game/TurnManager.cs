using ConnectFour.AI;
using ConnectFour.BoardGame;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ConnectFour.Game
{
    public enum PlayerType
    {
        None,
        Human, 
        AI
    }
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _pawnButtons;
        [SerializeField]
        private Grid _grid;
        [SerializeField]
        private ConnectFourAI _ai;
        [SerializeField]
        private PawnButtonHandler _PawnButtonHandler;
        [SerializeField]
        private Transform _coinSpawnPosition;


        #region EVENTS
        public static event Action<PawnOwner> OnColorChoose;                // Raised when the player choose a color;
        public static event Action StartGame;
        #endregion
        private PlayerType[] _playersType;
        private PawnOwner[] _players;
        private PawnOwner _playerChoice;
        private bool _isPlayer1Turn = true;                                 // Player 1 is alway Red, and start the game
        private bool _isWin = false;
        private GameObject _coin;                                            // Coin used for coin flip
        public bool IsWin { get => _isWin; }
        public bool IsPlayer1Turn { get => _isPlayer1Turn; }
        public PawnOwner PlayerChoice{ get => _playerChoice;}
        public PawnOwner ActivePlayer                                       // Return the player who is playing
        {
            get
            {
                return _isPlayer1Turn ? _players[0] : _players[1] ;
            }
        }
        public PawnOwner PassivePlayer                                      // Return the player who is waiting
        {
            get
            {
                return _isPlayer1Turn ? _players[1] : _players[0];
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _playersType = new PlayerType[2];                           // Two players
            _playersType[0] = PlayerType.AI;
            _playersType[1] = PlayerType.AI;

            _players = new PawnOwner[2];                                // We init the players tab
            _players[0] = PawnOwner.Player1;
            _players[1] = PawnOwner.Player2;                        
            _coin = Instantiate(_grid.Data.CoinTossPrefab, _coinSpawnPosition.position, Quaternion.identity);

        }
        private void OnEnable()
        {
            Grid.SwitchTurn += SwitchTurn;
            Grid.GameEnd += GameWin;
            StartGame += LaunchGame;
        }
        private void OnDisable()
        {
            Grid.SwitchTurn -= SwitchTurn;
            Grid.GameEnd -= GameWin;
            StartGame -= LaunchGame;
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void PlayerChooseRed(bool redIsChosen)
        {
            _playersType[redIsChosen ? 0 : 1] = PlayerType.Human;
           _playerChoice = redIsChosen ? PawnOwner.Player1 : PawnOwner.Player2;
            OnColorChoose?.Invoke(redIsChosen ? PawnOwner.Player1 : PawnOwner.Player2);
            StartCoroutine(CoinFlip());
        }

        public IEnumerator CoinFlip()
        {
            int startingPlayer = UnityEngine.Random.Range(0, 2);
            Debug.Log("Starting Player = " + startingPlayer);
            _coin.GetComponentInChildren<Animator>()?.SetTrigger(startingPlayer==0?"Red":"Yellow");
            Debug.Log("Color choose = " + _playerChoice.ToString());
            _PawnButtonHandler.InitButton(PlayerChoice);
            yield return new WaitForSeconds(2);

            _players[0] = startingPlayer == 0 ? PawnOwner.Player1 : PawnOwner.Player2;
            _players[1] = startingPlayer == 0 ? PawnOwner.Player2 : PawnOwner.Player1;

            StartGame?.Invoke();
        }
    
        public void LaunchGame()
        {
            _isPlayer1Turn = true;
            _ai.gameObject.SetActive(true);
            if (_players[0] == _playerChoice)
            {
                _pawnButtons.SetActive(true);
            }
            else
            {
                _ai.PlayAITurn();
            }
        }
        public void SwitchTurn()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
            if (ActivePlayer == _playerChoice)
            {
                _pawnButtons.SetActive(true);
            }
            else
            {
                _pawnButtons.SetActive(false);
            }
        }
        public void GameWin(PawnOwner player)
        {
            _isWin = true;
            _pawnButtons.SetActive(false);
        }
    }
}