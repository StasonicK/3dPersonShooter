﻿using System;
using UnityEngine;

namespace CodeBase.Logic.Hero.Animations
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] public Animator _animator;

        private readonly int _horizontalInputHash = Animator.StringToHash("HorizontalInput");
        private readonly int _verticalInputHash = Animator.StringToHash("VerticalInput");
        private readonly int _walkHash = Animator.StringToHash("Walking");
        private readonly int _runHash = Animator.StringToHash("Running");
        private readonly int _jumpHash = Animator.StringToHash("Jumping");
        private readonly int _idleStateHash = Animator.StringToHash("IdleState");
        private readonly int _walkingTreeStateHash = Animator.StringToHash("WalkingTree");
        private readonly int _runningTreeStateHash = Animator.StringToHash("RunningTree");
        private readonly int _jumpingTreeStateHash = Animator.StringToHash("JumpingTree");
        private CharacterController _characterController;

        public AnimatorState State { get; private set; }
        public bool IsJumping => State == AnimatorState.Jumping;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        private void Awake() =>
            _characterController = GetComponent<CharacterController>();

        private void Start()
        {
            PlayIdle();
        }

        public void PlayIdle()
        {
            _animator.SetBool(_walkHash, false);
            _animator.SetBool(_runHash, false);
        }

        public void ResetToIdle()
        {
            _animator.Play(_idleStateHash);
        }

        public void PlayWalk()
        {
            _animator.SetBool(_walkHash, true);
            _animator.SetFloat(_horizontalInputHash, _characterController.velocity.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(_verticalInputHash, _characterController.velocity.y, 0.1f, Time.deltaTime);
        }

        public void PlayRun()
        {
            _animator.SetBool(_runHash, true);
            _animator.SetFloat(_horizontalInputHash, _characterController.velocity.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(_verticalInputHash, _characterController.velocity.y, 0.1f, Time.deltaTime);
        }


        public void PlayJump()
        {
            _animator.SetTrigger(_jumpHash);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _walkingTreeStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _runningTreeStateHash)
                state = AnimatorState.Running;
            else if (stateHash == _jumpingTreeStateHash)
                state = AnimatorState.Jumping;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}