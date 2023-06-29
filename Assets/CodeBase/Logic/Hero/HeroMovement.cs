using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float _walkForwardSpeed = 3f;
        [SerializeField] private float _walkBackSpeed = 2f;
        [SerializeField] private float _runForwardSpeed = 7f;
        [SerializeField] private float _runBackSpeed = 5f;

        private const float MinimumMagnitude = 0.01f;

        private PlayerInput _playerInput;
        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private bool _isGrounded;
        private Vector3 _velocity;
        [HideInInspector] public Vector3 Direction;

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
            Vector2 movementInput = _playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 airDirection = Vector3.zero;

            if (_characterController.isGrounded)
                airDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
            else
                Direction = transform.forward * movementInput.y + transform.right * movementInput.x;

            Debug.Log($"x: {movementInput.x}");
            Debug.Log($"y: {movementInput.y}");

            if (movementInput.magnitude > MinimumMagnitude)
            {
                if (_playerInput.Player.Run.IsPressed())
                {
                    _characterController.Move((Direction.normalized * MovementSpeed(Direction.normalized, true) +
                                               airDirection.normalized * _walkBackSpeed) * Time.deltaTime);
                    _heroAnimator.SetHorizontalInput(movementInput.y);
                    _heroAnimator.SetVerticalInput(movementInput.x);
                    _heroAnimator.PlayRun();
                }
                else
                {
                    _characterController.Move((Direction.normalized * MovementSpeed(Direction.normalized, false) +
                                               airDirection.normalized * _walkBackSpeed) * Time.deltaTime);
                    _heroAnimator.SetHorizontalInput(movementInput.y);
                    _heroAnimator.SetVerticalInput(movementInput.x);
                    _heroAnimator.PlayWalk();
                }
            }
            else
            {
                _heroAnimator.SetHorizontalInput(movementInput.y);
                _heroAnimator.SetVerticalInput(movementInput.x);
                _heroAnimator.PlayIdle();
            }
        }

        private float MovementSpeed(Vector3 direction, bool isRun)
        {
            if (isRun)
            {
                if (direction.x > 0f)
                    return _runForwardSpeed;
                else
                    return _runBackSpeed;
            }
            else
            {
                if (direction.x > 0f)
                    return _walkForwardSpeed;
                else
                    return _walkBackSpeed;
            }
        }
    }
}