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
                .ObserveEveryValueChanged(agent => agent.remainingDistance)
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(remainingDistance =>
                {
                    _model.Animator
                        .SetTrigger(remainingDistance > _model.Agent.stoppingDistance ? 
                            Animations.Run : Animations.Idle);
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }
    }
}