using Cinemachine;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAiming))]
    public class HeroFOV : MonoBehaviour
    {
        [SerializeField] private float _fovSmoothSpeed = 10;
        [SerializeField] private float _adsFov = 40f;

        private HeroAiming _heroAiming;
        private CinemachineVirtualCamera _virtualCamera;
        private float _currentFov;
        private float _hipFov;
        private bool _isHipFire;

        private void Awake()
        {
            _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            _heroAiming = GetComponent<HeroAiming>();
            _heroAiming.ToHip += SetHip;
            _heroAiming.ToAim += SetAim;
        }

        private void Start()
        {
            _hipFov = _virtualCamera.m_Lens.FieldOfView;
            SetHip();
        }

        private void SetHip() =>
            _isHipFire = true;

        private void SetAim() =>
            _isHipFire = false;

        private void Update()
        {
            _currentFov = _isHipFire ? _hipFov : _adsFov;

            _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView, _currentFov,
                _fovSmoothSpeed * Time.deltaTime);
        }
    }
}