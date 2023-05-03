using ConnectFour.BoardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Hovering
{
    public class SelectionHover : MonoBehaviour
    {
        [SerializeField]
        private PawnOwner _player;
        [SerializeField]
        private Light _light;
        public event Action<PawnOwner> OnSelected;
        // Start is called before the first frame update
        void Start()
        {
            _light.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnMouseOver()
        {
            if (!_light.enabled)
            {
                _light.enabled= true;
            }
        }
        private void OnMouseExit()
        {
            if (_light.enabled)
            {
                _light.enabled = false;
            }
        }
        private void OnMouseUpAsButton()
        {
            OnSelected?.Invoke(_player);
        }
    }
}
