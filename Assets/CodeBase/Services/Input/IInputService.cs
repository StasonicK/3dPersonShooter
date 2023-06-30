using System;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        public event Action Shot;
        public event Action Jumped;
        public event Action SwitchedSide;

        Vector2 MoveAxis { get; }
        Vector2 LookAxis { get; }
        bool IsRunButtonUp();
        void Enable();
        void Disable();
        bool IsAimButtonUp();
    }
}