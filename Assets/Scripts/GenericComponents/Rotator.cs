using System;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Component which rotates gameobject
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        private bool _isRotating;
        private float _currentSpeed;
        /// <summary>
        /// Set the rotation state of the object
        /// </summary>
        /// <param name="shouldRotate">Should the object be rotating</param>
        /// <param name="speed">how fast should it be rotating</param>
        public void SetRotateState(in bool shouldRotate, in float speed)
        {
            _isRotating = shouldRotate;
            _currentSpeed = speed;
        }

        private void Update()
        {
            if (_isRotating)
                transform.Rotate(Vector3.up, _currentSpeed*Time.deltaTime);
        }
    }
}