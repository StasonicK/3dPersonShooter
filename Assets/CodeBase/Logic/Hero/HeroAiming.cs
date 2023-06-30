using System;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroAiming : MonoBehaviour
    {
        [SerializeField] private LayerMask _collidableLayers;
        [SerializeField] private float _aimSmoothSpeed = 20;
        [SerializeField] private float _maxDistance = 50f;

        private const float CentralPosition = 0.5f;

        private IInputService _inputService;
        public Transform AimPosition;
        private HeroAnimator _heroAnimator;

        public event Action ToHip;
        public event Action ToAim;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

        private void OnEnable() =>
            _inputService.Enable();

        private void OnDisable() =>
            _inputService.Disable();

        private void Update()
        {
            // if (_inputService.IsAimButtonUp())
            // {
            //     ToAim?.Invoke();
            //     _heroAnimator.PlayAim();
            // }
            // else
            // {
            //     ToHip?.Invoke();
            //     _heroAnimator.PlayHipFire();
            // }

            RotateToScreenCenter();
        }

        private void RotateToScreenCenter()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(CentralPosition, CentralPosition, 0));
            var targetPosition = MaxDistancePosition(ray);
            AimPosition.position =
                Vector3.Lerp(AimPosition.position, targetPosition, _aimSmoothSpeed * Time.deltaTime);
        }

        private Vector3 MaxDistancePosition(Ray ray)
        {
            RaycastHit[] results = new RaycastHit[1];
            int count = Physics.RaycastNonAlloc(ray, results, _maxDistance, _collidableLayers);
            return count > 0 ? results[0].point : ray.GetPoint(_maxDistance);
        }
    }
}