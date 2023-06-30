using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadSceneState : IPayloadedState<Scene>
    {
        private const string LevelName = "Level_";

        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private Scene _scene;
        private bool _isInitial = true;
        private GameObject _hud;

        public LoadSceneState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(Scene scene)
        {
            _scene = scene;

            if (_scene.ToString().Contains(LevelName))
                _loadingCurtain.Show();

            _sceneLoader.Load(_scene, OnLoaded);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        private void OnLoaded(Scene scene) =>
            _stateMachine.Enter<GameLoopState>();
    }
}