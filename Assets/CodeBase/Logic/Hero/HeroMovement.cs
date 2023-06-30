using CodeBase.Services;
using CodeBase.Services.Input;
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


        private IInputService _inputService;
        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private bool _isGrounded;
        private Vector3 _velocity;
        [HideInInspector] public Vector3 Direction;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

        private void OnEnable() =>
            _inputService.Enable();

        private void OnDisable() =>
            _inputService.Disable();

        private void Update() =>
            Move();

        private void Move()
        {
            Vector2 movementInput = _inputService.MoveAxis;
            Vector3 airDirection = Vector3.zero;

            if (_characterController.isGrounded)
                airDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
            else
                Direction = transform.forward * movementInput.y + transform.right * movementInput.x;

            if (movementInput.magnitude > Constants.MinimumMagnitude)
            {
                if (_inputService.IsRunButtonUp())
                    SetRun(airDirection, movementInput);
                else
                    SetWalk(airDirection, movementInput);
            }
            else
            {
                SetIdle(movementInput);
            }
        }

        private void SetRun(Vector3 airDirection, Vector2 movementInput)
        {
            _characterController.Move((Direction.normalized * GetMovementSpeed(Direction.normalized, true) +
                                       airDirection.normalized * _walkBackSpeed) * Time.deltaTime);
            _heroAnimator.SetHorizontalInput(movementInput.y);
            _heroAnimator.SetVerticalInput(movementInput.x);
            _heroAnimator.PlayRun();
        }

        private void SetWalk(Vector3 airDirection, Vector2 movementInput)
        {
            _characterController.Move((Direction.normalized * GetMovementSpeed(Direction.normalized, false) +
                                       airDirection.normalized * _walkBackSpeed) * Time.deltaTime);
            _heroAnimator.SetHorizontalInput(movementInput.y);
            _heroAnimator.SetVerticalInput(movementInput.x);
            _heroAnimator.PlayWalk();
        }

        private void SetIdle(Vector2 movementInput)
        {
            _heroAnimator.SetHorizontalInput(movementInput.y);
            _heroAnimator.SetVerticalInput(movementInput.x);
            _heroAnimator.PlayIdle();
        }

        private float GetMovementSpeed(Vector3 direction, bool isRun)
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