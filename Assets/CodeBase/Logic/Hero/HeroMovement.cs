using System;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;

        private CharacterController _characterController;
        private Transform _cameraMain;
        private bool _isGrounded;
        private Vector3 _velocity;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _characterController = GetComponent<CharacterController>();
        }

        private void Start() =>
            _cameraMain = UnityEngine.Camera.main.transform;

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update() =>
            Move();

        private void Move()
        {
            _isGrounded = _characterController.isGrounded;

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0;

            Vector2 movementInput = _playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 move = (_cameraMain.forward * movementInput.y + _cameraMain.right * movementInput.x);
            move.y = 0f;
            // Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
            _characterController.Move(move * _movementSpeed * Time.deltaTime);

            if (move != Vector3.zero)
                transform.forward = move;

            if (_playerInput.Player.Jump.triggered && _isGrounded)
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);

            _velocity.y += _gravityValue * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}