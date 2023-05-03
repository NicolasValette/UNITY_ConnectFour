using System.Collections;
using System.Collections.Generic;
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
            if (playerChoice == PawnOwner.Player1)
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
        // Update is called once per frame
        void Update()
        {

        }
    }
}