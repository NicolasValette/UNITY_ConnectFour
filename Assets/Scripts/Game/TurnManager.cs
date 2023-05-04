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

    /// <summary>
    /// Hold the turn sitching.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        #region Serilized attributes
        [SerializeField]
        private GameObject _pawnButtons;
        [SerializeField]
        private BoardGame.Grid _grid;
        [SerializeField]
        private ConnectFourAI _ai;
        [SerializeField]
        private PawnButtonHandler _PawnButtonHandler; 
        [SerializeField]
        private CameraController _cameraController;
        [Space]
        [Tooltip("Position where swpawn coin flip")]
        [SerializeField]
        private Transform _coinSpawnPosition;
        [Space]
        [Header("Debug")]
        [SerializeField]
        private bool _isAIInsteadOfHuman = false;
        #endregion

        #region EVENTS
        public static event Action<PawnOwner> OnColorChoose;                // Raised when the player choose a color;
        public static event Action StartGame;                               // Raised on game start
        public static event Action<PawnOwner> OnBeginnerChoose;             // Raised when player choose which color to play
        #endregion
     //   private PlayerType[] _playersType;
        private PawnOwner[] _players;
        private PawnOwner _playerChoice;
        private bool _isPlayer1Turn = true;                                 // Player 1 is alway Red, and start the game
        private bool _isWin = false;
        private bool _isPaused = false;
        private bool _activePlayerCanPlay;
        private GameObject _coin;                                            // Coin used for coin flip
        #region GETTERS
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
        #endregion
        // Start is called before the first frame update
        void Start()
        {
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
     
        // Old method, keep it here to stability
        public void PlayerChooseRed(bool redIsChosen)
        {
            _playerChoice = redIsChosen ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow;
            OnColorChoose?.Invoke(redIsChosen ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow); //event a enlevé
            StartCoroutine(CoinFlip());
        }

        /// <summary>
        /// Method to set the player choice.
        /// </summary>
        /// <param name="playerChoice">The choice of the player</param>
        public void PlayerChoose(PawnOwner playerChoice)
        {
            _playerChoice = playerChoice;
            _cameraController.MoveCameraToPosition();
            OnColorChoose?.Invoke(playerChoice);
            StartCoroutine(CoinFlip());
        }


        public IEnumerator CoinFlip()
        {
            // We choose who start the game.
            int startingPlayer = UnityEngine.Random.Range(0, 2);
            Debug.Log("Starting Player = " + startingPlayer);
            // Launch the good animation of the chosen beginner.
            _coin.GetComponentInChildren<Animator>()?.SetTrigger(startingPlayer==0?"Red":"Yellow");
            Debug.Log("Color choose = " + _playerChoice.ToString());
            // We initialize the button, depending of the beginner.
            _PawnButtonHandler.InitButton(PlayerChoice);
            // We wait the end af the animation.
            yield return new WaitForSeconds(2);

            // We raised the corresponding events.
            OnBeginnerChoose?.Invoke(startingPlayer == 0 ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow);
            _players[0] = startingPlayer == 0 ? PawnOwner.PlayerRed : PawnOwner.PlayerYellow;
            _players[1] = startingPlayer == 0 ? PawnOwner.PlayerYellow : PawnOwner.PlayerRed;

            //Start the game.
            StartGame?.Invoke();
        }
    
        /// <summary>
        /// Method to hold a lock between plays to prevent AI or player to play twice in a row.
        /// </summary>
       public void PlayerPlay()
        {
            _activePlayerCanPlay = false;
        }

        /// <summary>
        /// Start the game.
        /// </summary>
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

        /// <summary>
        /// Switch current player.
        /// </summary>
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