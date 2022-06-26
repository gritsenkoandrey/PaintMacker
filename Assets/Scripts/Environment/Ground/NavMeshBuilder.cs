using BaseMonoBehaviour;
using Managers;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Environment.Ground
{
    public sealed class NavMeshBuilder : BaseComponent
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        private MWorld _world;
        
        protected override void Awake()
        {
            base.Awake();
            
            _world = Manager.Resolve<MWorld>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _world.PassedGround
                .ObserveReset()
                .Subscribe(_ =>
                {
                    _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
                })
                .AddTo(LifeTimeDisposable);
        }

        protected override void Start()
        {
            base.Start();
            
            _navMeshSurface.BuildNavMesh();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            LifeTimeDisposable.Clear();
        }
    }
}