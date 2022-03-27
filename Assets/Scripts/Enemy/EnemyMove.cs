using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Enemy
{
    public sealed class EnemyMove : IEnemy
    {
        private readonly EnemyModel _model;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public EnemyMove(EnemyModel model)
        {
            _model = model;
        }
        
        public void Register()
        {
            float delay = 2f;
            
            _model.OnGeneratePath
                .Subscribe(_ =>
                {
                    delay = 2f;
                    
                    Vector3 hit = GeneratePointOnNavMesh(5f);
                    
                    _model.Agent.SetDestination(hit);
                })
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(_ =>
                {
                    if (_model.Agent.hasPath) return;

                    delay -= Time.deltaTime;
                    
                    if (delay > 0) return;
                    
                    _model.OnGeneratePath.Execute();
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
        }

        private Vector3 GeneratePointOnNavMesh(float radius)
        {
            NavMesh.SamplePosition(_model.Transform.position + GeneratePoint(radius), out NavMeshHit h, 10f, 1);
            
            return h.position;
        }

        private Vector3 GeneratePoint(float radius)
        {
            float angle = U.Random(0f, 1f) * (2f * Mathf.PI) - Mathf.PI;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 point = new Vector3(x, 0f, z);
            
            return point;
        }
    }
}