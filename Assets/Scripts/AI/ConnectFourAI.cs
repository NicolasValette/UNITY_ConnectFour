using ConnectFour.BoardGame;
using ConnectFour.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.AI
{
    public class ConnectFourAI: MonoBehaviour
    {
        [SerializeField]
        private TurnManager _turnManager;
        [SerializeField]
        private Grid _grid;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!_turnManager.IsPlayer1Turn && !_turnManager.IsWin)
            {
                //List<int> availableCell = new List<int>();
                //for (int i = 0; i < _grid.Data.Columns; i++)
                //{
                //    if (_grid.PlayingGrid[_grid.Data.Rows-1, i] == PawnOwner.None)  //We check if the columns is not full, 
                //    {
                //        availableCell.Add(i);
                //    }
                //}
                //_grid.PutPawn(availableCell[Random.Range(0, availableCell.Count)]); //then we pick one cell in the avalaible cells

                _grid.PutPawn(ChoosePawnToPlay()); 
            }
        }

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
