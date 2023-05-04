using ConnectFour.BoardGame;
using ConnectFour.Game;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.AI
{
    public class ConnectFourAI : MonoBehaviour
    {
        [SerializeField]
        private TurnManager _turnManager;
        [SerializeField]
        private BoardGame.Grid _grid;
        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);                                            // Disable IA before the game start
        }

        /// <summary>
        /// Debug method to allow AI to play in place of the player
        /// </summary>
        public void PlayInsteadOfPlayer()
        {
            if (!_turnManager.IsWin)
            {
                _grid.PutPawn(ChoosePawnToPlay());
            }
        }

        /// <summary>
        /// Method call to let AI play his turn
        /// </summary>
        public void PlayAITurn()
        {
           
            if (_turnManager.ActivePlayer != _turnManager.PlayerChoice && !_turnManager.IsWin)
            {
                _grid.PutPawn(ChoosePawnToPlay());
            }
            else if (_turnManager.IsAInsteadOfPlayer)
            {
                PlayInsteadOfPlayer();
            }
        }

        /// <summary>
        /// The main method of the AI of the game, check every colomn to verify if either AI or player can win, and play to win or block
        /// </summary>
        /// <returns>The number of column where to play</returns>
        private int ChoosePawnToPlay()
        {
            List<int> availableCell = new List<int>();
            int opponentWinningCol = -1;
            for (int col = 0; col < _grid.Data.Columns; col++)
            {
                if (_grid.IsColumnAvailable(col))
                {
                    availableCell.Add(col);
                    int row = 0;
                    while (_grid.PlayingGrid[row, col] != PawnOwner.None)
                    {
                        row++;
                    }
                    if (_grid.IsGameWin(row, col, _turnManager.ActivePlayer))           // Check if we win after this play    
                    {
                        return col;                                                     // if yes, we play here
                    }
                    else if (_grid.IsGameWin(row, col, _turnManager.PassivePlayer))     // Check if opponent win after this play
                    {
                        opponentWinningCol = col;                                       // If yes, save this to play it if we can't win this turn
                    }
                }
            }
            // If we can't win, we try to deny other player victory, otherwise, we play a random column 
            return (opponentWinningCol != -1) ? opponentWinningCol : availableCell[Random.Range(0, availableCell.Count)];
        }
    }
}
