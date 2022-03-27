using DG.Tweening;
using UnityEngine;

namespace Environment.Traps
{
    [System.Serializable]
    public sealed class TrapModel
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _ray;
        [SerializeField] private TrapSettings _settings;

        public GameObject GameObject => _gameObject;
        public Transform Transform => _transform;
        public TrapSettings TrapSettings => _settings;
        public Ray Ray => new Ray { origin = _ray.position, direction = Vector3.down };
        
        public Sequence Sequence;
    }
}