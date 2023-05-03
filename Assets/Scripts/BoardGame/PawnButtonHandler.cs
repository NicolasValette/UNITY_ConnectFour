using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ConnectFour.BoardGame
{
    public class PawnButtonHandler : MonoBehaviour
    {

        [SerializeField]
        private List<Transform> _buttonPositions;
        [SerializeField]
        private GameObject _redButtonPrefab;
        [SerializeField]
        private GameObject _yellowButtonPrefab;

        private Grid _grid;

        private int _actualKeyboardColumn = 0;
        // Start is called before the first frame update
        void Awake()
        {
            _grid = GetComponentInParent<Grid>();
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
        public void ActivateColumn()
        {
            _buttonPositions[_actualKeyboardColumn].GetComponentInChildren<Hover>()?.Activate();
        }
        public void SwitchColumn(int nextColumn)
        {
            _buttonPositions[_actualKeyboardColumn].GetComponentInChildren<Hover>()?.Exit();
            _actualKeyboardColumn = nextColumn;
            _buttonPositions[_actualKeyboardColumn].GetComponentInChildren<Hover>()?.Over(); ;
        }
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