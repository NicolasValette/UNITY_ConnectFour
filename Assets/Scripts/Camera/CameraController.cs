using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectFour.Camera
{
    [RequireComponent(typeof(Animator))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public void MoveCameraToPosition()
        {
            _animator.SetTrigger("StartGame");
        }
    }
}