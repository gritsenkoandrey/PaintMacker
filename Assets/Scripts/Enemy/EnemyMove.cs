using Managers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Enemy
{
    public sealed class EnemyMove : IEnemy
    {
        private readonly EnemyModel _model;
        
        private readonly MConfig _config;
        private readonly MPool _pool;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public EnemyMove(EnemyModel model)
        {
            _model = model;
            
            _config = Manager.Resolve<MConfig>();
            _pool = Manager.Resolve<MPool>();
        }
        
        public void Register()
        {
            float delay = _model.Delay;

            _model.EndPath = InitEndPath().transform;
            
            _model.OnGeneratePath
                .Subscribe(_ =>
                {
                    delay = _model.Delay;
                    
                    Vector3 hit = GeneratePointOnNavMesh(_model.Radius);

                    _model.EndPath.position = hit;

                    _model.Agent.SetDestination(hit);
                })
                .AddTo(_disposable);

            _model.Agent
                .UpdateAsObservable()
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(_ =>
                {
                    if (_model.Agent.hasPath) return;

                    delay -= Time.deltaTime;
                    
                    if (delay > 0) return;
                    
                    _model.OnGeneratePath.Execute();
                })
                .AddTo(_disposable);

            _model.Agent
                .ObserveEveryValueChanged(agent => agent.hasPath)
                .Where(_ => _model.Agent.isOnNavMesh)
                .Subscribe(hasPath =>
                {
                    _model.EndPath.gameObject.SetActive(hasPath);
                })
                .AddTo(_disposable);
        }

        public void Unregister()
        {
            _disposable.Clear();
            
            if (_model.EndPath) _pool.ReleaseObject(_model.EndPath.gameObject);
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
            
            return new Vector3(x, 0f, z);
        }

        private GameObject InitEndPath()
        {
            return _pool.SpawnObject(_config.EnvironmentData.EnemyTarget, 
                _model.Transform.position, Quaternion.identity);
        }
    }
}