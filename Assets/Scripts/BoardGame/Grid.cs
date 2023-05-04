using ConnectFour.Data;
using ConnectFour.Game;
using System;
using UnityEngine;

namespace ConnectFour.BoardGame
{
    public class Grid : MonoBehaviour
    {
        [Tooltip("Data of the grid")]
        [SerializeField]
        private GridData _gridData;
        [SerializeField]
        private TurnManager _turnManager;
        [Header("Stack of pawn")]
        [SerializeField]
        private Transform _PlayerPawnPos;
        [SerializeField]
        private Transform _OpponentPawnPos;
        [SerializeField]
        private GameObject _redStack;
        [SerializeField]
        private GameObject _yellowStack;


        #region EVENTS
        public static event Action SwitchTurn;          // Raised each turn to switch
        public static event Action<PawnOwner> GameEnd;  // Raised when a player win the game
        public static event Action GameDraw;            // Raised when the grid is full without winner


        private PawnOwner[,] _grid;
        private Pawn _actualPawn;                       // we keep the last instantiate pawn to play particle effect

        #endregion
        #region Getters
        public GridData Data { get => _gridData; }
        public PawnOwner[,] PlayingGrid { get => _grid; }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            _grid = new PawnOwner[_gridData.Rows, _gridData.Columns];
        }

        private void OnEnable()
        {
            Hover.OnHover += PutPawn;
            TurnManager.OnColorChoose += DisplayPlayerChoice;
        }
        private void OnDisable()
        {
            Hover.OnHover -= PutPawn;
            TurnManager.OnColorChoose -= DisplayPlayerChoice;
        }
        public void DisplayPlayerChoice(PawnOwner choice)
        {
            if (choice == PawnOwner.PlayerRed)
            {
                _redStack.transform.position = _PlayerPawnPos.position;
                _yellowStack.transform.position = _OpponentPawnPos.position;
            }
            else
            {
                _redStack.transform.position = _OpponentPawnPos.position;
                _yellowStack.transform.position = _PlayerPawnPos.position;
            }
        }
        private Vector2 GetPositionFromGrid(int rows, int columns)
        {
      
            Vector2 pos = new Vector2(_gridData.ColumnsPos[columns], _gridData.RowsPos[rows]);      // We switch pos to sync with Unity coord
            return pos;

        }
        public void ResetGrid()
        {
            for (int i = 0; i < _gridData.Rows; i++)
            {
                for (int j = 0; j < _gridData.Columns; j++)
                {
                    _grid[i, j] = PawnOwner.None;
                }
            }
        }
        // Put a pawn in a collumns, return the rows where the pawn fall, -1 if the columns is full
        public int AddPawn(int columns, PawnOwner player)
        {
            for (int i = 0; i < _gridData.Rows; i++)
            {
                if (_grid[i, columns] == PawnOwner.None)
                {
                    _grid[i, columns] = player;
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Check is this coord is in the grid.
        /// </summary>
        /// <param name="position">Position to check.</param>
        /// <returns>Returns true if present, else return false</returns>
        private bool IsCoordInGrid(Vector2 position)
        {
            if (position.x >= _gridData.Rows || position.y >= _gridData.Columns)
            {
                return false;
            }
            if (position.x < 0 || position.y < 0)
            {
                return false;
            }
            return (_grid[(int)position.x, (int)position.y] != PawnOwner.None);
        }

        /// <summary>
        /// Check the number of pawn aligned in the given direction.
        /// </summary>
        /// <param name="lastPawn">position of the last pawn played, starting point of theverification</param>
        /// <param name="dir">simple Vecor of the direction to check.</param>
        /// <param name="player">The player to check.</param>
        /// <returns>Number of aligned pawn in this direction</returns>
        private int CheckDirectedAlignment(Vector2 lastPawn, Vector2 dir, PawnOwner player)
        {
            int alignNumber = 1;
            bool positiveDirectioEnded = false;
            bool oppositeDirectionEnded = false;
            for (int i = 1; i < _gridData.NumberForWin; i++)
            {
                Vector2 tempVector = (lastPawn + (dir * i));
                Vector2 tempOppositeVector = (lastPawn - (dir * i));
                if (!positiveDirectioEnded && IsCoordInGrid(tempVector) && (_grid[(int)tempVector.x, (int)tempVector.y] == player))
                {
                    alignNumber++;
                }
                else
                {
                    // we reach the end of a line, with the end of grid or with an opponent pawn
                    positiveDirectioEnded = true;
                }
                if (!oppositeDirectionEnded && IsCoordInGrid(tempOppositeVector) && (_grid[(int)tempOppositeVector.x, (int)tempOppositeVector.y] == player))
                {
                    alignNumber++;
                }
                else
                {
                    // we reach the end of a line, with the end of grid or with an opponent pawn
                    oppositeDirectionEnded = true;
                }

                if (positiveDirectioEnded && oppositeDirectionEnded)
                {
                    return alignNumber;
                }
            }
            return alignNumber;
        }

        /// <summary>
        /// Check if the game is win with the last pawn played
        /// </summary>
        /// <param name="lastPawnRow">Row of the last played pawn.</param>
        /// <param name="lastPawnColumn">Column of the last played pawn.</param>
        /// <param name="player">The player who played the pawn.</param>
        /// <returns>True if is win.</returns>
        public bool IsGameWin(int lastPawnRow, int lastPawnColumn, PawnOwner player)
        {
            Vector2 lastPawnPos = new Vector2(lastPawnRow, lastPawnColumn);

            if ((CheckDirectedAlignment(lastPawnPos, new Vector2(0, 1), player) >= _gridData.NumberForWin) ||
                (CheckDirectedAlignment(lastPawnPos, new Vector2(1, 0), player) >= _gridData.NumberForWin) ||
                (CheckDirectedAlignment(lastPawnPos, new Vector2(1, 1), player) >= _gridData.NumberForWin) ||
                (CheckDirectedAlignment(lastPawnPos, new Vector2(1, -1), player) >= _gridData.NumberForWin))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the given column is full.
        /// </summary>
        /// <param name="column">The column to check.</param>
        /// <returns>true if is available, else false</returns>
        public bool IsColumnAvailable(int column)
        {
            return _grid[_gridData.Rows - 1, column] == PawnOwner.None;
        }

        public void PutPawn(int column)
        {
            if (IsColumnAvailable(column) && _turnManager.ActivePlayerCanPlay)
            {
                _turnManager.PlayerPlay();
                int row = AddPawn(column, _turnManager.ActivePlayer);
                Vector2 pos = GetPositionFromGrid(row, column);
                GameObject Pawn = Instantiate(_turnManager.ActivePlayer == PawnOwner.PlayerRed ? _gridData.Player1PawnPrefab : _gridData.Player2PawnPrefab,
                    new Vector3(pos.x, _gridData.YPosEntryPoint, 0f), Quaternion.identity);
                Pawn.transform.SetParent(transform, true);
                
                _actualPawn = Pawn.GetComponent<Pawn>();
                if (_actualPawn != null)
                {
                    _actualPawn.Fall(pos.y, row, column, EndOfTurnCheck);
                }


            }
        }
        public void EndOfTurnCheck(int lastRowPlayed, int lastColumnPlayed)
        {
            if (IsGameWin(lastRowPlayed, lastColumnPlayed, _turnManager.ActivePlayer))
            {
                _actualPawn.PlayWinEffect();
                GameEnd?.Invoke(_turnManager.ActivePlayer);
            }
            else
            {
                SwitchTurn?.Invoke();
            }


            int _numberOfColumFull = 0;
            for (int j = 0; j < _gridData.Columns; j++)
            {
                if (_grid[_gridData.Rows - 1, j] != PawnOwner.None)
                {
                    _numberOfColumFull++;                           // We track the number of pawn in the highest row
                }                                                   // if the highest line is full, the grid is full and the game is a draw
            }
            if (_numberOfColumFull >= _gridData.Columns)
            {
                GameEnd?.Invoke(PawnOwner.None);                    // Raised if the game is a draw
            }


        }
    }
}