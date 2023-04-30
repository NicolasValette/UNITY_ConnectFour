using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.BoardGame
{
    public enum PawnOwner
    {
        None,
        Player1,
        Player2
    }
    // Class to describe pawn behaviour
    public class Pawn : MonoBehaviour
    {
        private int _columnNumber;
        private int _rowNumber;
        private bool _isFalling = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Set this pawn in the grid at the correct column and the row where the pawn must fall
        public void PutPawnOnGrid(int row, int column)
        {
            _columnNumber = column;
            _rowNumber = row;
        }
    }
}
