using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour
{
    public class Hover : MonoBehaviour
    {
        [SerializeField]
        private int _column;
        #region EVENTS
        public static event Action<int> OnHover;
        #endregion

        private bool _isMouseOver;

        // Components we need later
        private Renderer _renderer;
        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnMouseExit()
        {
            if (_isMouseOver)
            {
                _isMouseOver = false;
                if (_renderer != null)
                {
                    _renderer.enabled = false;
                }
            }
        }
        private void OnMouseOver()
        {
            if (!_isMouseOver)
            {
                _isMouseOver = true;
                if (_renderer != null)
                {
                    _renderer.enabled = true;
                }
            }

        }
        private void OnMouseUpAsButton()
        {
            OnHover?.Invoke(_column);
        }
    }
}