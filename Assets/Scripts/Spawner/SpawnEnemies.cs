using BaseMonoBehaviour;
using Enemy;
using Managers;
using UnityEngine;

namespace Spawner
{
    public sealed class SpawnEnemies : BaseComponent
    {
        private MConfig _config;
        private MWorld _world;
        
        protected override void Init()
        {
            base.Init();
            
            InstantiateEnemy().Construct();
        }

        protected override void Enable()
        {
            base.Enable();

            _config = Manager.Resolve<MConfig>();
            _world = Manager.Resolve<MWorld>();
        }

        protected override void Disable()
        {
            base.Disable();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawSphere(transform.position, 0.25f);
        }

        private EnemyBehaviour InstantiateEnemy() => 
            Instantiate(_config.EnvironmentData.EnemyBehaviour, transform.position, 
                Quaternion.identity, _world.CurrentLevel.Value.transform);
    }
}