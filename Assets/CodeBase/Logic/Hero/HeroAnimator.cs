using System;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] public Animator _animator;

        private readonly int _horizontalInputHash = Animator.StringToHash("HorizontalInput");
        private readonly int _verticalInputHash = Animator.StringToHash("VerticalInput");
        private readonly int _walkHash = Animator.StringToHash("Walking");
        private readonly int _runHash = Animator.StringToHash("Running");
        private readonly int _jumpHash = Animator.StringToHash("Jumping");
        private readonly int _aimHash = Animator.StringToHash("Aiming");
        private readonly int _idleStateHash = Animator.StringToHash("IdleState");
        private readonly int _walkingTreeStateHash = Animator.StringToHash("WalkingTree");
        private readonly int _runningTreeStateHash = Animator.StringToHash("RunningTree");
        private readonly int _jumpingTreeStateHash = Animator.StringToHash("JumpingTree");

        public AnimatorState State { get; private set; }

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        private void Start() =>
            PlayIdle();

        public void SetHorizontalInput(float value) =>
            _animator.SetFloat(_horizontalInputHash, value);

        public void SetVerticalInput(float value) =>
            _animator.SetFloat(_verticalInputHash, value);

        public void PlayIdle()
        {
            _animator.SetBool(_walkHash, false);
            _animator.SetBool(_runHash, false);
        }

        public void PlayWalk()
        {
            _animator.SetBool(_walkHash, true);
            _animator.SetBool(_runHash, false);
        }

        public void PlayRun() => 
            _animator.SetBool(_runHash, true);

        public void PlayJump() => 
            _animator.SetTrigger(_jumpHash);

        public void PlayAim() =>
            _animator.SetBool(_aimHash, true);

        public void PlayHipFire() =>
            _animator.SetBool(_aimHash, false);

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