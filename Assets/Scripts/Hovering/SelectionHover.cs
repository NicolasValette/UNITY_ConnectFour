using ConnectFour.BoardGame;
using System;
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
        private Collider _collider;
        // Start is called before the first frame update
        void Start()
        {
            _light.enabled = false;
            _collider = GetComponent<Collider>();
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
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }
    }
}
