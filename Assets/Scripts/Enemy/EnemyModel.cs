using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [System.Serializable]
    public sealed class EnemyModel
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _ray;
        [SerializeField] private float _delay;
        [SerializeField] private float _radius;
        
        public Transform EndPath { get; set; }
        public NavMeshAgent Agent => _agent;
        public Animator Animator => _animator;
        public Transform Transform => _transform;
        public Ray Ray => new Ray { origin = _ray.position, direction = Vector3.down };
        public float Delay => _delay;
        public float Radius => _radius;

        public readonly ReactiveCommand OnGeneratePath = new ReactiveCommand();
    }
}