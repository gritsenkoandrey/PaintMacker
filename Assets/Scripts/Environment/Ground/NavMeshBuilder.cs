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
        
        protected override void Init()
        {
            base.Init();
            
            _navMeshSurface.BuildNavMesh();
            
            _world.PassedGround
                .ObserveReset()
                .Subscribe(_ =>
                {
                    _navMeshSurface.BuildNavMesh();
                })
                .AddTo(lifetimeDisposable);
        }

        protected override void Enable()
        {
            base.Enable();

            _world = Manager.Resolve<MWorld>();
        }

        protected override void Disable()
        {
            base.Disable();
        }
    }
}