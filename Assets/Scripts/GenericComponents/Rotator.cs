using UnityEngine;

namespace Mutations.GenericComponents
{
    /// <summary>
    ///     Component which rotates gameobject
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        private float _currentSpeed;
        private bool _isRotating;

        private void Update()
        {
            if (_isRotating)
                transform.Rotate(Vector3.up, _currentSpeed * Time.deltaTime);
        }

        /// <summary>
        ///     Set the rotation state of the object
        /// </summary>
        /// <param name="shouldRotate">Should the object be rotating</param>
        /// <param name="speed">how fast should it be rotating</param>
        public void SetRotateState(in bool shouldRotate, in float speed)
        {
            _isRotating = shouldRotate;
            _currentSpeed = speed;
        }
    }
}