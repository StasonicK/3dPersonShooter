using Cinemachine;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    public class HeroRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 4f;
        [SerializeField] private float _fovSmoothSpeed = 10;
        [SerializeField] private float _aimSmoothSpeed = 20;
        [SerializeField] private float _shoulderSwapSpeed = 10;
        [SerializeField] private float _adsFov = 40f;
        [SerializeField] private LayerMask _aimMask;
        [SerializeField] private Transform _cameraFollowPosition;

        private const int MaxVerticalAngle = 70;

        public Transform AimPosition;
        private PlayerInput _playerInput;
        private float xAxis, yAxis;
        private Animator _animator;
        private CinemachineVirtualCamera _virtualCamera;
        private float _hipFov;
        private float _currentFov;
        private float _xFollowPosition;
        private float _yFollowPosition;
        private float _ogYPosition;

        private void Awake()
        {
            _playerInput = new PlayerInput();
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
            _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _hipFov = _virtualCamera.m_Lens.FieldOfView;
            _animator = GetComponent<Animator>();
        }

        private void Rotate()
        {
            Vector2 delta = _playerInput.Player.Look.ReadValue<Vector2>();

            // as long as there is no aim mode
            _currentFov = _adsFov;

            xAxis += delta.x * _rotationSpeed;
            yAxis -= delta.y * _rotationSpeed;
            yAxis = Mathf.Clamp(yAxis, -MaxVerticalAngle, MaxVerticalAngle);

            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView, _currentFov,
                _fovSmoothSpeed * Time.deltaTime);

            Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCentre);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _aimMask))
                AimPosition.position = Vector3.Lerp(AimPosition.position, hit.point, _aimSmoothSpeed * Time.deltaTime);

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
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                _xFollowPosition = -_xFollowPosition;

            _yFollowPosition = _ogYPosition;

            Vector3 newFollowPos =
                new Vector3(_xFollowPosition, _yFollowPosition, _cameraFollowPosition.localPosition.z);
            _cameraFollowPosition.localPosition = Vector3.Lerp(_cameraFollowPosition.localPosition, newFollowPos,
                _shoulderSwapSpeed * Time.deltaTime);
        }
    }
}