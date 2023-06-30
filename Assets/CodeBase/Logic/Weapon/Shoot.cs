using CodeBase.Logic.Hero;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Weapon
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _barrelPosition;
        [SerializeField] private float _bulletVelocity;
        [SerializeField] private AudioClip _gunShot;
        [SerializeField] private float _lightReturnSpeed = 20;

        [HideInInspector] public AudioSource _audioSource;
        private IInputService _inputService;
        private HeroAiming _heroAiming;
        private Light _muzzleFlashLight;
        private ParticleSystem _muzzleFlashParticles;
        private float _lightIntensity;
        private float _fireRateTimer;
        private bool _clicked;
        private bool _needShoot;

        private void Awake() =>
            _inputService = AllServices.Container.Single<IInputService>();

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _heroAiming = GetComponentInParent<HeroAiming>();
            _muzzleFlashLight = GetComponentInChildren<Light>();
            _muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
            _lightIntensity = _muzzleFlashLight.intensity;
            _muzzleFlashLight.intensity = 0;
            _fireRateTimer = _fireRate;
        }

        private void OnEnable()
        {
            _inputService.Enable();
            _inputService.Shot += Shot;
        }

        private void OnDisable()
        {
            _inputService.Disable();
            _inputService.Shot -= Shot;
        }

        private void Update()
        {
            if (CanFire())
                Fire();

            _muzzleFlashLight.intensity =
                Mathf.Lerp(_muzzleFlashLight.intensity, 0, _lightReturnSpeed * Time.deltaTime);
        }

        private void Shot() =>
            _needShoot = true;

        private bool CanFire()
        {
            _fireRateTimer += Time.deltaTime;

            if (_fireRateTimer < _fireRate)
                return false;

            if (_needShoot)
            {
                _needShoot = false;
                return true;
            }

            return false;
        }

        private void Fire()
        {
            _fireRateTimer = 0;

            _barrelPosition.LookAt(_heroAiming.AimPosition);

            _audioSource.PlayOneShot(_gunShot);
            TriggerMuzzleFlash();

            GameObject currentBullet = Instantiate(_bullet, _barrelPosition.position, _barrelPosition.rotation);
            currentBullet.GetComponentInChildren<Rigidbody>()
                .AddForce(_barrelPosition.forward * _bulletVelocity, ForceMode.Impulse);
        }

        private void TriggerMuzzleFlash()
        {
            _muzzleFlashParticles.Play();
            _muzzleFlashLight.intensity = _lightIntensity;
        }
    }
}