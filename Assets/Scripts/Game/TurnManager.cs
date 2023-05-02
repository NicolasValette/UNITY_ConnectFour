using ConnectFour.BoardGame;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
        private Transform _coinSpawnPosition;

        private PlayerType[] _playersType;
        private PawnOwner[] _players;
        private bool _isPlayer1Turn = true;                                 // Player 1 is alway Red, and start the game
        private bool _isWin = false;
        private GameObject _coin;                                            // Coin used for coin flip
        public bool IsWin { get => _isWin; }
        public bool IsPlayer1Turn { get => _isPlayer1Turn; }
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
           // _coin = Instantiate(_grid.Data.CoinTossPrefab, _coinSpawnPosition.position, Quaternion.identity);

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
        public void PlayerChooseRed(bool redIsChosen)
        {
            _playersType[redIsChosen ? 0 : 1] = PlayerType.Human;
            StartCoroutine(CoinFlip());
        }

        public IEnumerator CoinFlip()
        {
            int startingPlayer = Random.Range(0, 2);
            Debug.Log("Starting Player = " + startingPlayer);
            _coin.GetComponentInChildren<Animator>()?.SetTrigger(startingPlayer==0?"Red":"Yellow");
            
            yield return new WaitForSeconds(2);

            _players[0] = startingPlayer == 0 ? PawnOwner.Player1 : PawnOwner.Player2;
            _players[1] = startingPlayer == 0 ? PawnOwner.Player2 : PawnOwner.Player1;
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