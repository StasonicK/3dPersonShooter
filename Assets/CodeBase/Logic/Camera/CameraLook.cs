using Cinemachine;
using UnityEngine;

namespace CodeBase.Logic.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class CameraLook : MonoBehaviour
    {
        [SerializeField] private float _lookSpeed = 1f;

        private CinemachineFreeLook _cinemachineFreeLook;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
            _playerInput = new PlayerInput();
        }

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update()
        {
            Vector2 delta = _playerInput.Player.Look.ReadValue<Vector2>();
            _cinemachineFreeLook.m_XAxis.Value += delta.x * 200 * _lookSpeed * Time.deltaTime;
        }
    }
}