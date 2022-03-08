using DG.Tweening;
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

        public NavMeshAgent Agent => _agent;
        public Animator Animator => _animator;
        public Transform Transform => _transform;
        public Transform Ray => _ray;
        
        public Tween Tween;

        public readonly ReactiveCommand OnGeneratePath = new ReactiveCommand();
    }
}