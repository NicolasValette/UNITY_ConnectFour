using ConnectFour;
using ConnectFour.BoardGame;
using ConnectFour.Data;
using ConnectFour.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private GridData _gridData;
    [SerializeField]
    private TurnManager _turnManager;

    private int _numberOfColumFull = 0;             // Track the number of column full, if this reach the number of max column, the game is a draw
    #region EVENTS
    public static event Action SwitchTurn;          // Raised each turn to switch
    public static event Action<PawnOwner> GameEnd;  // Raised when a player win the game
    public static event Action GameDraw;            // Raised when the grid is full without winner
        
    private PawnOwner[,] _grid;
    
    #endregion
    #region Getters
    public GridData Data { get => _gridData; }
    public PawnOwner[,] PlayingGrid { get => _grid; }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _grid = new PawnOwner[_gridData.Rows,_gridData.Columns]; 
    }

    private void OnEnable()
    {
        Hover.OnHover += PutPawn;
    }
    private void OnDisable()
    {
        Hover.OnHover -= PutPawn;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector2 GetPositionFromGrid(int rows, int columns)
    {
        //float deltaX = 1f / _gridData.Columns ;
        //float deltaY = (_gridData.YPosHighestRow - _gridData.YPosLowestRow) / _gridData.Rows;
        //Vector2 pos = new Vector2((-0.5f) + (deltaX * columns), (_gridData.YPosLowestRow + (deltaY * rows)));

        Vector2 pos = new Vector2( _gridData.ColumnsPos[columns], _gridData.RowsPos[rows]); 
    
        return pos;
        
    }
    public void ResetGrid()
    {
        for (int i = 0; i < _gridData.Rows; i++)
        {
            for (int j = 0; j< _gridData.Columns; j++)
            {
                _grid[i,j] = PawnOwner.None;
            }
        }
        _numberOfColumFull = 0;
    }
    // Put a pawn in a collumns, return the rows where the pawn fall, -1 if the columns is full
    public int AddPawn (int columns, PawnOwner player)
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

    private bool IsGameWin(int lastPawnRow, int lastPawnColumn, PawnOwner player)
    {
        int line = 1; //Number of pan in line.

        // first, check horizontal aligment
        //step 1 : left side
        for (int i = lastPawnColumn - 1; i >= 0; i--)
        {
            if (_grid[lastPawnRow, i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        //step 2 : right side
        for (int i = lastPawnColumn + 1; i < _gridData.Columns; i++)
        {
            if (_grid[lastPawnRow, i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        if (line >= _gridData.NumberForWin)
        {
            return true;
        }

        // Check Diagonal
        line = 1;                                   // We reset the number of pawn, we check other direction
        // Check North West Side
        for (int i=1; (lastPawnColumn - i >= 0 && lastPawnRow + i < _gridData.Rows); i++)
        {
            if (_grid[lastPawnRow + i, lastPawnColumn - i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        // Check South East Side
        for (int i = 1; (lastPawnColumn + i < _gridData.Columns && lastPawnRow - i >=0); i++)
        {
            if (_grid[lastPawnRow - i, lastPawnColumn + i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        if (line >= _gridData.NumberForWin)
        {
            return true;
        }

        // Check other Diagonal
        line = 1;                                   // We reset the number of pawn, we check other direction
        // Check North East Side
        for (int i = 1; (lastPawnColumn + i < _gridData.Columns && lastPawnRow + i < _gridData.Rows); i++)
        {
            if (_grid[lastPawnRow + i, lastPawnColumn + i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        // Check South West Side
        for (int i = 1; (lastPawnColumn - i >= 0 && lastPawnRow - i >= 0); i++)
        {
            if (_grid[lastPawnRow - i, lastPawnColumn - i] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        if (line >= _gridData.NumberForWin)
        {
            return true;
        }

        // Check Vertical
        line = 1;                                   // we reset the number on pawn lined up
        for (int i = 1; lastPawnRow - i >= 0; i++)
        {
            if (_grid[lastPawnRow - i, lastPawnColumn] == player)    // Check if the neihgour box contain player pawn
            {
                line++;
            }
            else
            {
                break;                              // If the box contains opponent pawn, we are going to check other side
            }
        }
        if (line >= _gridData.NumberForWin)
        {
            return true;
        }
        return false;
    }

    public bool IsColumnAvailable(int column)
    {
        bool result = _grid[_gridData.Rows - 1, column] == PawnOwner.None;
        if (!result)                                // if this is false, the column is full
        {
            _numberOfColumFull++;
        }
        return _grid[_gridData.Rows -1, column] == PawnOwner.None;
    }
    public void PutPawn(int column)
    {
        if (IsColumnAvailable(column))
        {
            int row = AddPawn(column, _turnManager.IsPlayer1Turn ? PawnOwner.Player1 : PawnOwner.Player2);
            //  Debug.Log("row = " + row);
            Vector2 pos = GetPositionFromGrid(row, column);
            GameObject Pawn = Instantiate(_turnManager.IsPlayer1Turn ? _gridData.Player1PawnPrefab : _gridData.Player2PawnPrefab,
                new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            Pawn.transform.SetParent(transform, true);

            if (IsGameWin(row, column, _turnManager.IsPlayer1Turn ? PawnOwner.Player1 : PawnOwner.Player2))
            {
                GameEnd?.Invoke(_turnManager.IsPlayer1Turn ? PawnOwner.Player1 : PawnOwner.Player2);
            }
            else
            {
                SwitchTurn?.Invoke();
            }
        }
        if (_numberOfColumFull >= _gridData.Columns)
        {
            GameEnd?.Invoke(PawnOwner.None);
        }
    }
}
