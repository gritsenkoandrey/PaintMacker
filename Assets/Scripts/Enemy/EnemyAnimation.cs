using UniRx;
using Utils;

namespace Enemy
{
    public sealed class EnemyAnimation : IEnemy
    {
        private readonly EnemyModel _model;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public EnemyAnimation(EnemyModel model)
        {
            _model = model;
        }

        public void Register()
        {
            _model.Agent
                .ObserveEveryValueChanged(agent => agent.hasPath)
                .Where(_ => _model.Agent.isOnNavMesh)
                .Skip(1)
                .Subscribe(hasPath =>
                {
                    _model.Animator.SetTrigger(hasPath ? Animations.Run : Animations.Idle);
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }
    }
}