using UnityEngine;

namespace CodeBase.Logic.Hero
{
    public class HeroRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 4f;

        private PlayerInput _playerInput;
        private Transform _child;
        private Transform _cameraMain;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _child = transform.GetChild(0).transform;
        }

        private void Start() => 
            _cameraMain = UnityEngine.Camera.main.transform;

        private void OnEnable() =>
            _playerInput.Enable();

        private void OnDisable() =>
            _playerInput.Disable();

        private void Update() => 
            Rotate();

        private void Rotate()
        {
            Vector2 movementInput = _playerInput.Player.Move.ReadValue<Vector2>();

            if (movementInput != Vector2.zero)
            {
                Quaternion rotation =
                    Quaternion.Euler(new Vector3(_child.localEulerAngles.x, _cameraMain.localEulerAngles.y,
                        _child.localEulerAngles.z));
                _child.rotation = Quaternion.Lerp(_child.rotation, rotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }
}