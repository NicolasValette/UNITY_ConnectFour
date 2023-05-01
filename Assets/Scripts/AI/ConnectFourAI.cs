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
                List<int> availableCell = new List<int>();
                for (int i = 0; i < _grid.Data.Columns; i++)
                {
                    if (_grid.PlayingGrid[_grid.Data.Rows-1, i] == PawnOwner.None)  //We check if the columns is not full, 
                    {
                        availableCell.Add(i);
                    }
                }
                _grid.PutPawn(availableCell[Random.Range(0, availableCell.Count)]); //then we pick one cell in the avalaible cells
            }
        }
    }
}
