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
                _grid.PutPawn(Random.Range(0, _grid.Data.Columns));
            }
        }
    }
}
