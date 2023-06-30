﻿using System;
using CodeBase.Logic.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic.Weapon
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private Button _shootButtonLeft;
        [SerializeField] private Button _shootButtonRight;
        [SerializeField] private float _fireRate;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _barrelPosition;
        [SerializeField] private float _bulletVelocity;
        [SerializeField] private AudioClip _gunShot;
        [SerializeField] private float _lightReturnSpeed = 20;

        [HideInInspector] public AudioSource _audioSource;
        private PlayerInput _playerInput;
        private HeroAiming _heroAiming;
        private Light _muzzleFlashLight;
        private ParticleSystem _muzzleFlashParticles;
        private float _lightIntensity;
        private float _fireRateTimer;
        private bool _clicked;

        private void Awake()
        {
            // _shootButtonLeft.onClick.AddListener(Clicked);
            // _shootButtonRight.onClick.AddListener(Clicked);
        }

        private void Clicked() =>
            _clicked = true;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _playerInput = new PlayerInput();
            _heroAiming = GetComponentInParent<HeroAiming>();
            _muzzleFlashLight = GetComponentInChildren<Light>();
            _muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
            _lightIntensity = _muzzleFlashLight.intensity;
            _muzzleFlashLight.intensity = 0;
            _fireRateTimer = _fireRate;
        }

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update()
        {
            if (CanFire())
                Fire();

            _muzzleFlashLight.intensity =
                Mathf.Lerp(_muzzleFlashLight.intensity, 0, _lightReturnSpeed * Time.deltaTime);
        }

        private bool CanFire()
        {
            _fireRateTimer += Time.deltaTime;

            if (_fireRateTimer < _fireRate)
                return false;

            if (_playerInput.Player.Shoot.IsPressed() || _clicked)
                return true;

            return false;
        }

        private void Fire()
        {
            _fireRateTimer = 0;

            _barrelPosition.LookAt(_heroAiming.AimPosition);

            _audioSource.PlayOneShot(_gunShot);
            TriggerMuzzleFlash();

            GameObject currentBullet = Instantiate(_bullet, _barrelPosition.position, _barrelPosition.rotation);

            Bullet bullet = currentBullet.GetComponent<Bullet>();
            bullet.Direction = _barrelPosition.transform.forward;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(_barrelPosition.forward * _bulletVelocity, ForceMode.Impulse);
        }

        private void TriggerMuzzleFlash()
        {
            _muzzleFlashParticles.Play();
            _muzzleFlashLight.intensity = _lightIntensity;
        }
    }
}