using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ConnectFour.BoardGame
{
    /// <summary>
    /// Hold method of the button of the grid
    /// </summary>
    public class PawnButtonHandler : MonoBehaviour
    {
        [Tooltip("List of position of all of the buttons.")]
        [SerializeField]
        private List<Transform> _buttonPositions;
        [Space]
        [Header("Prefabs")]
        [SerializeField]
        private GameObject _redButtonPrefab;
        [SerializeField]
        private GameObject _yellowButtonPrefab;

        private Grid _grid;
        private int _actualKeyboardColumn = 0;

        private List<Hover> _buttonPositionHoverComponentInChildren;
      
        void Awake()
        {
            _grid = GetComponentInParent<Grid>();
        }
        private void Start()
        {
            _buttonPositionHoverComponentInChildren = new List<Hover>();
            for (int i = 0; i < _buttonPositions.Count; i++)
            {
                Hover component = _buttonPositions[i].GetComponentInChildren<Hover>();
                if (component != null)
                {
                    _buttonPositionHoverComponentInChildren.Add(component);
                }
                else
                {
                    Debug.Log("Missing componen in PawnButtonHandler, button number " + i);
                }
                
            }
        }
        private void OnEnable()
        {
            Hover.OnHover += DisableButtons;
        }
        private void OnDisable()
        {
            Hover.OnHover -= DisableButtons;
        }
        public void InitButton(PawnOwner playerChoice)
        {
            GameObject prefabToInstantiate;
            if (playerChoice == PawnOwner.PlayerRed)
            {
                prefabToInstantiate = _redButtonPrefab;
            }
            else
            {
                prefabToInstantiate = _yellowButtonPrefab;
            }
            for (int i = 0; i < _buttonPositions.Count; i++)
            {
                GameObject button = Instantiate(prefabToInstantiate, Vector3.zero, Quaternion.identity);
                button.GetComponent<Hover>()?.InitColumn(i);
                button.transform.SetParent(_buttonPositions[i]);
            }
        }

        public void EnableButtons()
        {
            gameObject.SetActive(true);
            for (int i=0; i<_grid.Data.Columns; i++)
            {
                // We check every colomn, and disable them if it's full
                if (!_grid.IsColumnAvailable(i))
                {
                    _buttonPositions[i].gameObject.SetActive(false);
                }
            }
            // We the actual column is full, we go to the right
            if (!_grid.IsColumnAvailable(_actualKeyboardColumn))
            {
                RightColumn();
            }

        }
        public void DisableButtons()
        {
            gameObject.SetActive(false);
        }
        // method who listen OnHoverEvent
        public void DisableButtons(int column)
        {
            DisableButtons();
        }
        /// <summary>
        /// Activate a column to put a pawn on it.
        /// </summary>
        public void ActivateColumn()
        {
            _buttonPositionHoverComponentInChildren[_actualKeyboardColumn].Activate();
        }
        public void SwitchColumn(int nextColumn)
        {
            for (int i=0; i<_grid.Data.Columns;i++)
            {
                _buttonPositionHoverComponentInChildren[i].Exit();
            }
            _actualKeyboardColumn = nextColumn;
            _buttonPositionHoverComponentInChildren[_actualKeyboardColumn].Over(); ;
        }
        /// <summary>
        /// Move the selected column to the right.
        /// </summary>
        public void RightColumn()
        {
            int nextColumn = 1;
            // we check each next column until we found an available column
            while (nextColumn <= _grid.Data.Columns && !_grid.IsColumnAvailable((_actualKeyboardColumn + nextColumn)% _grid.Data.Columns))
            {
                nextColumn++;
            }
            SwitchColumn((_actualKeyboardColumn + nextColumn) % _grid.Data.Columns);           
        }

        /// <summary>
        /// Move the selected column to the left.
        /// </summary>
        public void LeftColumn()
        {
            int nextColumn = _actualKeyboardColumn -1;
            int j = 0;      // number of checked column
            if (nextColumn <= -1)
            {
                nextColumn = _grid.Data.Columns -1;
            }
            // we check each next column until we found an available column
            while (j <= _grid.Data.Columns && !_grid.IsColumnAvailable(nextColumn))
            {
                nextColumn--;
                if (nextColumn <= -1)
                {
                    nextColumn = _grid.Data.Columns - 1;
                }
                j++;
            }
            SwitchColumn(nextColumn);
        }
    }
} 