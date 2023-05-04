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
        [Tooltip("Required pawn needed to win")]
        [SerializeField]
        private int _numberForWin = 4;
        [Space]
        [Header ("Entry Position of the rows")]
        [SerializeField]
        private float _yPosEntryPoint;
        [Space]
        [Header("Cell coord")]
        [Tooltip("List of coord of each rows")]
        [SerializeField]
        private List<float> _rowsPos;
        [Tooltip("List of coord of each columns")]
        [SerializeField]
        private List<float> _columnsPos;
        [Space]
        [Header("Default Materials")]
        [SerializeField]
        private Material _defaultRedMat;
        [SerializeField]
        private Material _defaultYellowMat;
        [Space]
        [Header("Prefabs")]
        [SerializeField]
        private GameObject _player1PawnPrefab;
        [SerializeField]
        private GameObject _player2PawnPrefab;
        [SerializeField]
        private GameObject _coinTossPrefab;
        

        #region Getters
        public int Columns { get => _columns; }
        public int Rows { get => _rows; }
        public int NumberForWin { get => _numberForWin; }

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