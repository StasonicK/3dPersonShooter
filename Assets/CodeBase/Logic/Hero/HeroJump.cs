using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroJump : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;

        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private bool _isGrounded;
        private Vector3 _velocity;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

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

            if (_playerInput.Player.Jump.triggered && _isGrounded)
            {
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
                _heroAnimator.PlayJump();
            }

            _velocity.y += _gravityValue * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}