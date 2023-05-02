using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Data
{
    [CreateAssetMenu(menuName = "Data/New Grid")]
    public class GridData : ScriptableObject
    {
        [Header("Size of the grid")]
        [Tooltip("Number of columns of our grid")]
        [SerializeField]
        private int _columns = 7;
        [Tooltip("Number of rows of our grid")]
        [SerializeField]
        private int _rows = 6;
        [SerializeField]
        private int _numberForWin = 4;

        [Space]
        [Header ("Position of the rows")]
        [Tooltip("Y position of the lowest row in the grid")]
        [SerializeField]
        private float _yPosLowestRow;
        [Tooltip("Y position of the highest row in the grid")]
        [SerializeField]
        private float _yPosHighestRow;
        [SerializeField]
        private float _yPosEntryPoint;
        [Space]
        [Header ("The pawns")]
        [SerializeField]
        private GameObject _player1PawnPrefab;
        [SerializeField]
        private GameObject _player2PawnPrefab;
        [SerializeField]
        private Material _defaultRedMat;
        [SerializeField]
        private Material _defaultYellowMat;
        [SerializeField]
        private GameObject _coinTossPrefab;
        [SerializeField]
        private List<float> _rowsPos;
        [SerializeField]
        private List<float> _columnsPos;

        #region Getters
        public int Columns { get => _columns; }
        public int Rows { get => _rows; }
        public int NumberForWin { get => _numberForWin; }
        public float YPosLowestRow { get => _yPosLowestRow;}
        public float YPosHighestRow { get => _yPosHighestRow;}
        public float YPosEntryPoint { get => _yPosEntryPoint; }
        public GameObject Player1PawnPrefab { get => _player1PawnPrefab;}
        public GameObject Player2PawnPrefab { get => _player2PawnPrefab; }
        public GameObject CoinTossPrefab { get => _coinTossPrefab; }
        public Material RedMaterial
        {
            get
            {
                Material mat = Player1PawnPrefab.GetComponent<Material> ();
                return (mat == null) ? _defaultRedMat : mat;
            }
        }
        public Material YellowMaterial
        {
            get
            {
                Material mat = Player2PawnPrefab.GetComponent<Material>();
                return (mat == null) ? _defaultYellowMat : mat;
            }
        }
        public List<float> RowsPos { get => _rowsPos; }
        public List<float> ColumnsPos { get => _columnsPos; }
        #endregion
    }
}