using UnityEngine;

namespace CodeBase.Logic.Hero.Animations
{
    public abstract class MovementBaseState : MonoBehaviour
    {
        public abstract void EnterState(MovementStateManager movement);

        public abstract void UpdateState(MovementStateManager movement);
    }
}