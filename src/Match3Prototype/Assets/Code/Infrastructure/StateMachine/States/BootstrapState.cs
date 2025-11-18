using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.StateMachine.Interfaces;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : BaseState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IAssetProvider _assetProvider;

        public BootstrapState(IStateMachine stateMachine, IAssetProvider assetProvider)
        {
            _stateMachine = stateMachine;
            _assetProvider = assetProvider;
        }

        public override async UniTask Enter()
        {
            await _assetProvider.InitializeAsync();
            await _stateMachine.Enter<LoadingGameState, string>(AssetLabels.GAME_SCENE);
        }
    }
}