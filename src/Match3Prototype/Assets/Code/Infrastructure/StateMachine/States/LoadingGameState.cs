using System;
using Code.Infrastructure.Loading;
using Code.Infrastructure.StateMachine.Interfaces;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadingGameState : BasePayloadState<string>
    {
        private readonly IStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        private const float _MIN_LOADING_TIME_SECONDS = 1.5f;

        public LoadingGameState(IStateMachine stateMachine, ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public override async UniTask Enter(string sceneName)
        {
            //ToDo: Add show loading view

            UniTask loadingTask = _sceneLoader.LoadScene(sceneName);
            UniTask minimalTimeTask = UniTask.Delay(TimeSpan.FromSeconds(_MIN_LOADING_TIME_SECONDS));

            await UniTask.WhenAll(loadingTask, minimalTimeTask);
            await _stateMachine.Enter<GameEnterState>();
        }

        public override UniTask Exit()
        {
            //ToDo: Add close loading view
            return base.Exit();
        }
    }
}