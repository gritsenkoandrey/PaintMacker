using BaseMonoBehaviour;
using Character;
using Managers;
using UnityEngine;

namespace Spawner
{
    public sealed class SpawnCharacter : BaseComponent
    {
        private MConfig _config;
        private MWorld _world;
        
        protected override void Init()
        {
            base.Init();

            CharacterBehaviour character = InstantiateCharacter();

            _world.Character = character;
            
            character.Construct();
            
            _world.CurrentLevel.Value.Cameras[1].Follow = character.transform;
            _world.CurrentLevel.Value.Cameras[1].LookAt = character.transform;
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
            Gizmos.color = Color.green;
            
            Gizmos.DrawSphere(transform.position, 0.25f);
        }

        private CharacterBehaviour InstantiateCharacter() => 
            Instantiate(_config.CharacterData.Character, transform.position, 
                Quaternion.identity, _world.CurrentLevel.Value.transform);
    }
}