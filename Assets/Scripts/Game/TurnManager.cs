using ConnectFour.AI;
using ConnectFour.BoardGame;
using ConnectFour.Camera;
using ConnectFour.UI;
using System;
using System.Collections;
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
        private BoardGame.Grid _grid;
        [SerializeField]
        private ConnectFourAI _ai;
        [SerializeField]
        private PawnButtonHandler _PawnButtonHandler;
        [SerializeField]
        private Transform _coinSpawnPosition;
        [SerializeField]
        private CameraController _cameraController;
        [SerializeField]
        private bool _isAIInsteadOfHuman = false;


        #region EVENTS
        public static event Action<PawnOwner> OnColorChoose;                // Raised when the player choose a color;
        public static event Action StartGame;
        public static event Action<PawnOwner> OnBeginnerChoose;
        #endregion
        private PlayerType[] _playersType;
        private PawnOwner[] _players;
        private PawnOwner _playerChoice;
        private bool _isPlayer1Turn = true;                                 // Player 1 is alway Red, and start the game
        private bool _isWin = false;
        private bool _isPaused = false;
        private bool _activePlayerCanPlay;
        private GameObject _coin;                                            // Coin used for coin flip
        public bool IsWin { get => _isWin; }
        public bool IsPaused { get => _isPaused; }
        public bool IsPlayer1Turn { get => _isPlayer1Turn; }
        public PawnOwner PlayerChoice{ get => _playerChoice;}
        public bool ActivePlayerCanPlay { get => _activePlayerCanPlay; }
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

        public PawnOwner StartingPlayer { get => _players[0]; }
        public bool IsAInsteadOfPlayer { get => _isAIInsteadOfHuman; }
        // Start is called before the first frame update
        void Start()
        {
            _playersType = new PlayerType[2];                           // Two players
            _playersType[0] = PlayerType.AI;
            _playersType[1] = PlayerType.AI;

            _players = new PawnOwner[2];                                // We init the players tab
            _players[0] = PawnOwner.PlayerRed;
            _players[1] = PawnOwner.PlayerYellow;                        
            _coin = Instantiate(_grid.Data.CoinTossPrefab, _coinSpawnPosition.position, Quaternion.identity);

        }
        private void OnEnable()
        {
            BoardGame.Grid.SwitchTurn += SwitchTurn;
            BoardGame.Grid.GameEnd += GameWin;
            StartGame += LaunchGame;
        }
        private void OnDisable()
        {
            BoardGame.Grid.SwitchTurn -= SwitchTurn;
            BoardGame.Grid.GameEnd -= GameWin;
            StartGame -= LaunchGame;
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void PlayerChooseRed(bool redIsChosen)
        {
            _playersType[redIsChosen ? 0 : 1] = PlayerType.Human;
           _playerChoice = redIsChosen ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow;
            OnColorChoose?.Invoke(redIsChosen ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow); //event a enlevé
            StartCoroutine(CoinFlip());
        }
        public void PlayerChoose(PawnOwner playerChoice)
        {
            _playerChoice = playerChoice;
            _cameraController.MoveCameraToPosition();
          //  _grid.DisplayPlayerChoice(playerChoice);
            OnColorChoose?.Invoke(playerChoice);
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
            OnBeginnerChoose?.Invoke(startingPlayer == 0 ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow);
            _players[0] = startingPlayer == 0 ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow;
            _players[1] = startingPlayer == 0 ? PawnOwner.PlayerYellow : PawnOwner.PlayerRed;

            StartGame?.Invoke();
        }
    
       public void PlayerPlay()
        {
            _activePlayerCanPlay = false;
        }
        public void LaunchGame()
        {
            _isPlayer1Turn = true;
            _activePlayerCanPlay = true;
            _ai.gameObject.SetActive(true);
            if (_players[0] == _playerChoice && !_isAIInsteadOfHuman )
            {
                _PawnButtonHandler.EnableButtons();
            }
            else
            {
                _ai.PlayAITurn();
            }
        }
        public void SwitchTurn()
        {
            _isPlayer1Turn = !_isPlayer1Turn;
            _activePlayerCanPlay = true;
            if (ActivePlayer == _playerChoice && !_isAIInsteadOfHuman)
            {
                _PawnButtonHandler.EnableButtons();
                //_pawnButtons.SetActive(true);
            }
            else
            {
                _pawnButtons.SetActive(false);
                _ai.PlayAITurn();
            }
        }
        public void GameWin(PawnOwner player)
        {
            _isWin = true;
            _pawnButtons.SetActive(false);
        }
    }
}