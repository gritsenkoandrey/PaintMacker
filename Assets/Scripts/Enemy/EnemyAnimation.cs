using UniRx;
using UnityEngine;
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
                .ObserveEveryValueChanged(agent => agent.velocity.sqrMagnitude)
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(velocity =>
                {
                    _model.Animator.SetFloat(Animations.Velocity, velocity, 0.05f, Time.deltaTime);
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }
    }
}