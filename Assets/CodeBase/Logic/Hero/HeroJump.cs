using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroJump : MonoBehaviour
    {
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;

        private IInputService _inputService;
        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private bool _isGrounded;
        private Vector3 _velocity;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }


        private void OnEnable()
        {
            _inputService.Enable();
            _inputService.Jumped += TryJump;
        }

        private void OnDisable()
        {
            _inputService.Disable();
            _inputService.Jumped -= TryJump;
        }

        private void Update() =>
            Move();

        private void TryJump()
        {
            if (_characterController.isGrounded)
            {
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
                Vector2 movementInput = _inputService.MoveAxis;
                _heroAnimator.SetHorizontalInput(movementInput.y);
                _heroAnimator.SetVerticalInput(movementInput.x);
                _heroAnimator.PlayJump();
            }
        }

        private void Move()
        {
            _isGrounded = _characterController.isGrounded;

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0;

            _velocity.y += _gravityValue * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}