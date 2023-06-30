using System;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public class InputService : IInputService
    {
        private PlayerInput _playerInput;

        public event Action Shot;
        public event Action Jumped;
        public event Action SwitchedSide;

        public InputService()
        {
            _playerInput = new PlayerInput();

            _playerInput.Player.Jump.started += x => { Jumped?.Invoke(); };
            _playerInput.Player.SwitchSide.started += x => { SwitchedSide?.Invoke(); };
            _playerInput.Player.Shoot.started += x => { Shot?.Invoke(); };

            _playerInput.Enable();
        }

        public void Enable() =>
            _playerInput.Enable();

        public void Disable() =>
            _playerInput.Disable();

        public Vector2 MoveAxis => _playerInput.Player.Move.ReadValue<Vector2>();

        public Vector2 LookAxis => _playerInput.Player.Look.ReadValue<Vector2>();

        public bool IsAimButtonUp() =>
            _playerInput.Player.Aim.IsPressed();

        public bool IsRunButtonUp() =>
            _playerInput.Player.Run.IsPressed();
    }
}