using CodeBase.Logic.Hero.Animations;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;

        private const float MinimumMagnitude = 0.01f;
        private const float RunMultiplayer = 2.0f;

        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private Transform _cameraMain;
        private bool _isGrounded;
        private Vector3 _velocity;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _characterController = GetComponent<CharacterController>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

        private void Start()
        {
            _cameraMain = UnityEngine.Camera.main.transform;
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

            Vector3 move = (_cameraMain.forward * movementInput.y + _cameraMain.right * movementInput.x);
            move.y = 0f;

            if (move != Vector3.zero)
                transform.forward = move;

            if (movementInput.magnitude > MinimumMagnitude)
            {
                if (_playerInput.Player.Run.IsPressed())
                {
                    _characterController.Move(move * _movementSpeed * RunMultiplayer * Time.deltaTime);
                    _heroAnimator.PlayRun();
                }
                else
                {
                    _characterController.Move(move * _movementSpeed * Time.deltaTime);
                    _heroAnimator.PlayWalk();
                }
            }
            else
            {
                _heroAnimator.PlayIdle();
            }
        }
    }
}