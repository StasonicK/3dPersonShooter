using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpHeight = 1.0f;
        [SerializeField] private float _gravityValue = -9.81f;

        private CharacterController _characterController;
        private bool _isGrounded;
        private Vector3 _velocity;

        // private IInputService _inputService;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _characterController = GetComponent<CharacterController>();
        }
        // _inputService = AllServices.Container.Single<IInputService>();

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update()
        {
            _isGrounded = _characterController.isGrounded;

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0;

            Vector2 movementInput = _playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
            _characterController.Move(move * _movementSpeed * Time.deltaTime);

            if (move != Vector3.zero)
                transform.forward = move;

            if (_playerInput.Player.Jump.triggered && _isGrounded)
                _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);

            _velocity.y += _gravityValue * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
            // Move();
        }

        private void Move()
        {
            Vector3 movementVector = Vector3.zero;

            // if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            // {
            //     movementVector = new Vector3(_inputService.Axis.x, 0f, _inputService.Axis.y);
            //     movementVector.y = 0;
            //     movementVector.Normalize();
            //
            //     transform.forward = movementVector;
            // }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }
    }
}