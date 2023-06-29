using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAiming))]
    public class HeroRotation : MonoBehaviour
    {
        [SerializeField] private float _hipRotationVerticalSpeed = 2f;
        [SerializeField] private float _hipRotationHorizontalSpeed = 4f;
        [SerializeField] private float _aimRotationVerticalSpeed = 1f;
        [SerializeField] private float _aimRotationHorizontalSpeed = 2f;
        [SerializeField] private float _shoulderSwapSpeed = 10;
        [SerializeField] private Transform _cameraFollowPosition;

        private const int MaxVerticalAngle = 70;

        private PlayerInput _playerInput;
        private HeroAiming _heroAiming;
        private float xAxis, yAxis;
        private float _xFollowPosition;
        private float _yFollowPosition;
        private float _ogYPosition;
        private float _currentRotationVerticalSpeed;
        private float _currentRotationHorizontalSpeed;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _heroAiming = GetComponent<HeroAiming>();
            _heroAiming.ToHip += SetHip;
            _heroAiming.ToAim += SetAim;
            SetHip();
        }

        private void SetHip()
        {
            _currentRotationVerticalSpeed = _hipRotationVerticalSpeed;
            _currentRotationHorizontalSpeed = _hipRotationHorizontalSpeed;
        }

        private void SetAim()
        {
            _currentRotationVerticalSpeed = _aimRotationVerticalSpeed;
            _currentRotationHorizontalSpeed = _aimRotationHorizontalSpeed;
        }

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update() =>
            Rotate();

        private void Start()
        {
            _xFollowPosition = _cameraFollowPosition.localPosition.x;
            _ogYPosition = _cameraFollowPosition.localPosition.y;
            _yFollowPosition = _ogYPosition;
        }

        private void Rotate()
        {
            Vector2 delta = _playerInput.Player.Look.ReadValue<Vector2>();

            xAxis += delta.x * _currentRotationHorizontalSpeed;
            yAxis -= delta.y * _currentRotationVerticalSpeed;
            yAxis = Mathf.Clamp(yAxis, -MaxVerticalAngle, MaxVerticalAngle);


            MoveCamera();
        }

        private void LateUpdate()
        {
            _cameraFollowPosition.localEulerAngles =
                new Vector3(yAxis, _cameraFollowPosition.localEulerAngles.y, _cameraFollowPosition.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
        }

        void MoveCamera()
        {
            if (_playerInput.Player.SwitchSide.IsPressed())
                _xFollowPosition = -_xFollowPosition;

            _yFollowPosition = _ogYPosition;

            Vector3 newFollowPos =
                new Vector3(_xFollowPosition, _yFollowPosition, _cameraFollowPosition.localPosition.z);
            _cameraFollowPosition.localPosition = Vector3.Lerp(_cameraFollowPosition.localPosition, newFollowPos,
                _shoulderSwapSpeed * Time.deltaTime);
        }
    }
}