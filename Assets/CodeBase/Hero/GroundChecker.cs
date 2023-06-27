using System;
using UnityEngine;

namespace CodeBase.Hero
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _groundYOffset;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector3 _spherePosition;
        [SerializeField] private float _gravity = -9.81f;

        private Vector3 _velocity;

        private void Update()
        {
            Gravity();
        }

        private bool IsGrounded()
        {
            _spherePosition = new Vector3(transform.position.x, transform.position.y - _groundYOffset,
                transform.position.z);

            if (Physics.CheckSphere(_spherePosition, _characterController.radius - 0.05f, _groundMask))
                return true;

            return false;
        }

        private void Gravity()
        {
            if (!IsGrounded())
                _velocity.y += _gravity * Time.deltaTime;
            else if (_velocity.y < 0)
                _velocity.y = -2;

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_spherePosition, _characterController.radius - 0.05f);
        }
    }
}