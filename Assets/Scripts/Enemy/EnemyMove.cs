using DG.Tweening;
using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Enemy
{
    public sealed class EnemyMove : IEnemy
    {
        private readonly EnemyModel _model;
        private readonly MWorld _world;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public EnemyMove(EnemyModel model)
        {
            _model = model;

            _world = Manager.Resolve<MWorld>();
        }
        
        public void Register()
        {
            Vector3 hit = default;

            Transform[] paths = _world.CurrentLevel.Value.EnemyPath.transform.ChildPoints();
            
            _model.OnGeneratePath
                .Subscribe(_ =>
                {
                    _model.Tween.KillTween();

                    if (_model.Agent is { hasPath: true })
                    {
                        _model.Agent.ResetPath();
                    }
                    
                    float angle = U.Random(0f, 1f) * (2f * Mathf.PI) - Mathf.PI;
            
                    const float radius = 2f;
                    
                    float x = Mathf.Cos(angle) * radius;
                    float z = Mathf.Sin(angle) * radius;

                    Vector3 point = new Vector3(x, 0f, z);

                    Vector3 curHit;

                    do
                    {
                        NavMesh.SamplePosition(paths.GetRandom().position + point, out NavMeshHit h, 10f, 1);
                    
                        curHit = h.position;
                        
                    } while ((curHit - hit).sqrMagnitude < 2.5f);
                    
                    hit = curHit;
                    
                    _model.Agent.SetDestination(hit);
                })
                .AddTo(_disposable);

            _model.Agent
                .ObserveEveryValueChanged(agent => agent.hasPath)
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(hasPath =>
                {
                    if (hasPath)
                    {
                        return;
                    }
                    
                    _model.Tween = DOVirtual
                        .DelayedCall(2f, () =>
                        {
                            _model.OnGeneratePath.Execute();
                        })
                        .SetUpdate(true);
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _model.Tween.KillTween();
            _disposable.Clear();
        }
    }
}