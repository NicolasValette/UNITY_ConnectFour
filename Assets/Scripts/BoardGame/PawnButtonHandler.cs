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

        // Start is called before the first frame update
        void Start()
        {

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
        // Update is called once per frame
        void Update()
        {

        }
    }
}