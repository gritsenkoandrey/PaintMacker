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
        public Transform Ray => _ray;
        public TrapSettings TrapSettings => _settings;

        public Sequence Sequence;
    }
}