using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Character
{
    [System.Serializable]
    public sealed class CharacterModel
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _path;
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _forward;

        [HideInPrefabs] public readonly ReactiveProperty<bool> IsMove = new ReactiveProperty<bool>();
        public readonly ReactiveCommand<bool> OnVictory = new ReactiveCommand<bool>();

        public readonly CompositeDisposable CharacterDisposable = new CompositeDisposable();

        public CharacterController CharacterController => _characterController;
        public Animator Animator => _animator;
        public Transform Transform => _transform;
        public Ray RayPath => new Ray { origin = _path.position, direction = Vector3.down };
        public Ray RayCenter => new Ray { origin = _center.position, direction = Vector3.down };
        public Ray RayForward => new Ray { origin = _forward.position, direction = Vector3.down };
    }
}