using System;
using Code.Gameplay.Presentation.Loading;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Loading;
using Code.Infrastructure.StateMachine.Interfaces;
using Code.Infrastructure.UI.Factory;
using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class LoadingGameState : BasePayloadState<string>
    {
        private const float _MIN_LOADING_TIME_SECONDS = 2f;

        private readonly IStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIFactory _uiFactory;

        private IView _view = null;

        public LoadingGameState(IStateMachine stateMachine, ISceneLoader sceneLoader, IUIFactory uiFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
        }

        public override async UniTask Enter(string sceneName)
        {
            _view = await _uiFactory.CreateView<LoadingView>(nameof(LoadingView));
            await _view.Show();

            UniTask loadingTask = _sceneLoader.LoadScene(sceneName);
            UniTask minimalTimeTask = UniTask.Delay(TimeSpan.FromSeconds(_MIN_LOADING_TIME_SECONDS));

            await UniTask.WhenAll(loadingTask, minimalTimeTask);
            await _stateMachine.Enter<GameEnterState>();
        }

        public override async UniTask Exit()
        {
            if (_view != null)
            {
                await _view.Hide();
                _uiFactory.CloseView(_view);
                _view = null;
            }
        }
    }
}