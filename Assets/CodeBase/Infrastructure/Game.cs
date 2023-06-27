using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Game : MonoBehaviour
    {
        public readonly GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            StateMachine =
                new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
        }
    }
}