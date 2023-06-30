using System;
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

        public Transform AimPosition;
        private PlayerInput _playerInput;
        private HeroAnimator _heroAnimator;

        public event Action ToHip;
        public event Action ToAim;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update()
        {
            if (_playerInput.Player.Aim.IsPressed())
            {
                ToAim?.Invoke();
                _heroAnimator.PlayAim();
            }
            else
            {
                ToHip?.Invoke();
                _heroAnimator.PlayHipFire();
            }

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